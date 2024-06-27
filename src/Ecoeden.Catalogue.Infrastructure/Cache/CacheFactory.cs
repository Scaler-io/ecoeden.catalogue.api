using Ecoeden.Catalogue.Application.Contracts.Cache;
using Ecoeden.Catalogue.Application.Factories;
using Ecoeden.Catalogue.Domain.Models.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace Ecoeden.Catalogue.Infrastructure.Cache;
internal class CacheFactory(IServiceProvider serviceProvider) : ICacheFactory
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public ICacheService CreateService(CacheServiceTypes type)
    {
        return type switch
        {
            CacheServiceTypes.Distributed => _serviceProvider.GetRequiredService<DistributedCacheService>(),
            CacheServiceTypes.InMemory => _serviceProvider.GetRequiredService<InMemoryCacheService>(),
            _ => throw new ArgumentException($"Unsupported cache service type: {type}", nameof(type))
        };
    }
}
