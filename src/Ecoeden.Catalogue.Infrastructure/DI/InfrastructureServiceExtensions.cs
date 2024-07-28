using Ecoeden.Catalogue.Application.Contracts.Data;
using Ecoeden.Catalogue.Application.Contracts.Data.Sql;
using Ecoeden.Catalogue.Application.Contracts.EventBus;
using Ecoeden.Catalogue.Application.Contracts.HealthStatus;
using Ecoeden.Catalogue.Application.Factories;
using Ecoeden.Catalogue.Domain.Configurations;
using Ecoeden.Catalogue.Infrastructure.Cache;
using Ecoeden.Catalogue.Infrastructure.Data;
using Ecoeden.Catalogue.Infrastructure.Data.Repositories;
using Ecoeden.Catalogue.Infrastructure.Data.Sql;
using Ecoeden.Catalogue.Infrastructure.Data.Sql.Repositories;
using Ecoeden.Catalogue.Infrastructure.EventBus;
using Ecoeden.Catalogue.Infrastructure.HealthStatus;
using MassTransit;
using Microsoft.EntityFrameworkCore;
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

        services.AddScoped(typeof(IPublishService<,>), typeof(PublishService<,>));
        services.AddScoped<IPublishServiceFactory, PublishServiceFactory>();

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

        

        services.AddMassTransit(config =>
        {
            config.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("catalogues", false));
            config.UsingRabbitMq((context, cfg) =>
            {
                var rabbitmq = configuration.GetSection(EventBusOptions.OptionName).Get<EventBusOptions>();
                cfg.Host(rabbitmq.Host, "/", host =>
                {
                    host.Username(rabbitmq.Username);
                    host.Password(rabbitmq.Password);
                });
            });
        });

        services.AddDbContext<EcoedenDbContext>(option =>
        {
            option.UseSqlServer(configuration.GetConnectionString("Sqlserver"));
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

        return services;
    }
}
