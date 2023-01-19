using Infrastructure;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Net.Http.Headers;
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

        services.AddCors(options => {
            options.AddPolicy(CorsConstants.AccessControlAllowOrigin, builder =>
                builder.WithOrigins("*")
                    .WithHeaders("*")
                    .WithMethods("*")
                    .WithExposedHeaders("Content-Disposition")
            );
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (Configuration["ComponentConfig:Environment"].Equals("Development")) {
            app.UseDeveloperExceptionPage();

            app.UseDirectoryBrowser();
        }
        else {
            app.UseExceptionHandler("/error");
        }

        app.UseSwagger();

        app.UseSwaggerUI(options => {
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

        app.UseAuthentication();

        app.UseAuthorization();

        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}