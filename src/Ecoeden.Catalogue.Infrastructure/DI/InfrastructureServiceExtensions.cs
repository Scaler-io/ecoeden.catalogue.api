using Ecoeden.Catalogue.Application.Contracts.Cache;
using Ecoeden.Catalogue.Application.Contracts.HealthStatus;
using Ecoeden.Catalogue.Application.Factories;
using Ecoeden.Catalogue.Infrastructure.Cache;
using Ecoeden.Catalogue.Infrastructure.HealthStatus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ecoeden.Catalogue.Infrastructure.DI;
public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfraServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ICacheFactory, CacheFactory>();
        services.AddScoped<ICacheService, DistributedCacheService>();

        services.AddScoped<IHealthCheck, DbHealthCheck>();
        services.AddScoped<IHealthCheckConfiguration, HealthCheckConfiguration>();

        services.AddStackExchangeRedisCache(options =>
        {
            options.InstanceName = configuration["Redis:InstanceName"];
            options.Configuration = configuration.GetConnectionString("Redis");
        });

        return services;
    }
}
