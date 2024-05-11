using Ecoeden.Catalogue.Application.Contracts.Cache;
using Ecoeden.Catalogue.Domain.Configurations;
using Ecoeden.Catalogue.Domain.Models.Enums;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Ecoeden.Catalogue.Infrastructure.Cache;
public sealed class DistributedCacheService(IDistributedCache distributedCache, IOptions<AppConfigOption> appOptions)
    : ICacheService
{
    private readonly IDistributedCache _distributedCache = distributedCache;
    private readonly AppConfigOption _appOptions = appOptions.Value;

    public CacheServiceTypes ServiceType { get; } = CacheServiceTypes.Distributed;

    public async Task<bool> ContainsAsync(string key, CancellationToken cancellation = default)
    {
        return (await _distributedCache.GetStringAsync(key, cancellation)) is not null;
    }

    public async Task<T> GetAsync<T>(string key, CancellationToken cancellation = default)
    {
        var data = await _distributedCache.GetStringAsync(key, cancellation);
        if (data is null)
        {
            return default;
        }

        return JsonConvert.DeserializeObject<T>(data);
    }

    public async Task RemoveAsync(string key, CancellationToken cancellation = default)
    {
        await _distributedCache.RemoveAsync(key, cancellation);
    }

    public async Task SetAsync<T>(string key, T value, int? expirationTime, CancellationToken cancellation = default)
    {
        var serializedData = JsonConvert.SerializeObject(value);

        var cacheOptions = new DistributedCacheEntryOptions();
        cacheOptions.SetAbsoluteExpiration(TimeSpan.FromMinutes(
                expirationTime ?? _appOptions.CacheExpiration
            ));
        await _distributedCache.SetStringAsync(key, serializedData, cacheOptions, cancellation);
    }

    public async Task<T> UpdateAsync<T>(string key, T data)
    {
        await _distributedCache.SetStringAsync(key, JsonConvert.SerializeObject(data));
        return await GetAsync<T>(key);
    }
}
