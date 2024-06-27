using Ecoeden.Catalogue.Application.Contracts.Data;
using Ecoeden.Catalogue.Application.Contracts.HealthStatus;
using Ecoeden.Catalogue.Application.Factories;
using Ecoeden.Catalogue.Domain.Configurations;
using Ecoeden.Catalogue.Infrastructure.Cache;
using Ecoeden.Catalogue.Infrastructure.Data;
using Ecoeden.Catalogue.Infrastructure.Data.Repositories;
using Ecoeden.Catalogue.Infrastructure.HealthStatus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Ecoeden.Catalogue.Infrastructure.DI;
public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfraServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ICacheFactory, CacheFactory>();
        services.AddScoped<DistributedCacheService>();
        services.AddScoped<InMemoryCacheService>();

        services.AddScoped<IHealthCheck, DbHealthCheck>();
        services.AddScoped<IHealthCheckConfiguration, HealthCheckConfiguration>();

        services.AddScoped(sp =>
        {
            var mongoOptions = sp.GetRequiredService<IOptions<MongoDbOption>>().Value;
            return new MongoClient(mongoOptions.ConnectionString);
        });

        services.AddStackExchangeRedisCache(options =>
        {
            options.InstanceName = configuration["Redis:InstanceName"];
            options.Configuration = configuration.GetConnectionString("Redis");
        });

        services.AddScoped(typeof(IDocumentRepository<>), typeof(MongoDocumentRepository<>));
        services.AddScoped<ICatalogueContext, CatalogueContext>();

        return services;
    }
}
