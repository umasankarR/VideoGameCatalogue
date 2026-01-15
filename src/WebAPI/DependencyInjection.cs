using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using WebAPI.Extensions;

namespace WebAPI;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddEndpoints(Assembly.GetExecutingAssembly());

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddAuthorization();

        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });

        return services;
    }
}

