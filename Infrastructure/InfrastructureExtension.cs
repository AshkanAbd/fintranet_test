using Infrastructure.Repositories.TodoItem;
using Infrastructure.Repositories.TodoList;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class InfrastructureExtension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options => {
            options.UseSqlServer(
                configuration.GetConnectionString("DatabaseConnection")
            );
            if (configuration["ComponentConfig:Environment"].Equals("Development")) {
                options.EnableSensitiveDataLogging();
            }
        });

        services.AddScoped<ITodoItemRepository, TodoItemRepository>();
        services.AddScoped<ITodoListRepository, TodoListRepository>();

        return services;
    }
}