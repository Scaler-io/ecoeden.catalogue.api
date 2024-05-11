using Ecoeden.Catalogue.Application.Contracts.Cache;
using Ecoeden.Catalogue.Application.Factories;
using Ecoeden.Catalogue.Domain.Models.Enums;

namespace Ecoeden.Catalogue.Infrastructure.Cache;
internal class CacheFactory(IEnumerable<ICacheService> cacheServices) : ICacheFactory
{
    private readonly IEnumerable<ICacheService> _cacheServices = cacheServices;

    public ICacheService GetService(CacheServiceTypes type)
    {
        return _cacheServices.FirstOrDefault(x => x.ServiceType == type);
    }
}
