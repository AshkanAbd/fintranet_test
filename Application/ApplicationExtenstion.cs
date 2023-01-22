using System.Reflection;
using System.Text.Json;
using Application.Common.Response;
using Application.TodoItem;
using Application.TodoList;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ApplicationExtension
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());

        services.AddTransient<ITodoItemService, TodoItemService>();
        services.AddTransient<ITodoListService, TodoListService>();

        return services;
    }

    public static IApplicationBuilder UseApplication(this IApplicationBuilder app)
    {
        app.UseStatusCodePages(async context => {
                switch (context.HttpContext.Response.StatusCode) {
                    case 401 when context.HttpContext.Response.ContentType != "application/json":
                        context.HttpContext.Response.ContentType = "application/json";
                        await context.HttpContext.Response.WriteAsync(
                            JsonSerializer.Serialize(ResponseFormat.NotAuth<object>().Value
                            )
                        );
                        break;
                    case 403 when context.HttpContext.Response.ContentType != "application/json":
                        context.HttpContext.Response.ContentType = "application/json";
                        await context.HttpContext.Response.WriteAsync(
                            JsonSerializer.Serialize(
                                ResponseFormat.PermissionDeniedMsg<object>("Access denied").Value
                            )
                        );
                        break;
                    case 400:
                        context.HttpContext.Response.ContentType = "application/json";
                        await context.HttpContext.Response.WriteAsync(
                            JsonSerializer.Serialize(ResponseFormat.BadRequestMsg<object>("Bad request").Value
                            )
                        );
                        break;
                    case 500:
                        context.HttpContext.Response.ContentType = "application/json";
                        await context.HttpContext.Response.WriteAsync(
                            JsonSerializer.Serialize(
                                ResponseFormat.InternalErrorMsg<object>("Server internal error").Value
                            )
                        );
                        break;
                }
            }
        );

        return app;
    }
}