using Ecoeden.Catalogue.Domain.Models.Enums;

namespace Ecoeden.Catalogue.Application.Contracts.Cache;
public interface ICacheService
{
    CacheServiceTypes ServiceType { get; }
    Task<T> GetAsync<T>(string key, CancellationToken cancellation = default);
    Task SetAsync<T>(string key, T value, int? expirationTime = null, CancellationToken cancellation = default);
    Task RemoveAsync(string key, CancellationToken cancellation = default);
    Task<bool> ContainsAsync(string key, CancellationToken cancellation= default);
    Task<T> UpdateAsync<T>(string key, T data);
}
