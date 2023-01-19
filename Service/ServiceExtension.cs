using Microsoft.Extensions.DependencyInjection;

namespace Service;

public static class ServiceExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        return services;
    }
}