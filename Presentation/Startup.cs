using System.Reflection;
using Application;
using Application.Common.Response;
using Infrastructure;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Presentation;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddInfrastructure(Configuration);

        services.AddApplication();

        services.AddCors(options => {
            options.AddPolicy(CorsConstants.AccessControlAllowOrigin, builder =>
                builder.WithOrigins("*")
                    .WithHeaders("*")
                    .WithMethods("*")
                    .WithExposedHeaders("Content-Disposition")
            );
        });

        services.AddControllers(
            options => options.RespectBrowserAcceptHeader = true
        ).ConfigureApiBehaviorOptions(options => {
            options.SuppressConsumesConstraintForFormFileParameters = true;
            options.InvalidModelStateResponseFactory = _ => ResponseFormat.BadRequestMsg<object>();
        }).AddNewtonsoftJson();

        services.AddSwaggerGen(options => {
            options.SwaggerDoc(
                "api",
                new OpenApiInfo {
                    Title = "TodoList",
                }
            );
            options.AddSecurityDefinition("Token", new OpenApiSecurityScheme {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme.",
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme {
                        Reference = new OpenApiReference {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Token",
                        },
                    },
                    new string[] { }
                }
            });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            options.IncludeXmlComments(xmlPath);
            options.CustomSchemaIds(x => x.ToString());
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (Configuration["Environment"].Equals("Development")) {
            app.UseDeveloperExceptionPage();

            app.UseDirectoryBrowser();
        }
        else {
            app.UseExceptionHandler("/error");
        }

        app.UseSwagger();

        app.UseSwaggerUI(options => {
            options.SwaggerEndpoint($"/swagger/api/swagger.json", "TodoList api");
            options.EnableFilter("");
            options.DocExpansion(DocExpansion.None);
            options.ShowExtensions();
            options.DefaultModelRendering(ModelRendering.Example);
            options.DisplayOperationId();
            options.DisplayRequestDuration();
            options.EnableDeepLinking();
            options.EnablePersistAuthorization();
            options.ShowCommonExtensions();
            options.EnableTryItOutByDefault();
        });

        app.UseApplication();

        app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithExposedHeaders("Content-Disposition")
        );

        app.UseStaticFiles(new StaticFileOptions {
            HttpsCompression = HttpsCompressionMode.Compress,
            OnPrepareResponse = context => {
                var headers = context.Context.Response.GetTypedHeaders();
                headers.CacheControl = new CacheControlHeaderValue {
                    Public = true,
                    MaxAge = TimeSpan.FromDays(180)
                };
            }
        });

        app.UseRouting();

        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}