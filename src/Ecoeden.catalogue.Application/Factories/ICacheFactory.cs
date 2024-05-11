using Ecoeden.Catalogue.Application.Contracts.Cache;
using Ecoeden.Catalogue.Domain.Models.Enums;

namespace Ecoeden.Catalogue.Application.Factories;
public interface ICacheFactory
{
    ICacheService GetService(CacheServiceTypes type);
}
