using Ecoeden.Catalogue.Application.Contracts.Cache;
using Ecoeden.Catalogue.Domain.Configurations;
using Ecoeden.Catalogue.Domain.Models.Enums;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Ecoeden.Catalogue.Infrastructure.Cache;
public class InMemoryCacheService(IMemoryCache memoryCache, IOptions<AppConfigOption> appConfigOptions) 
    : ICacheService
{
    private readonly IMemoryCache _memoryCache = memoryCache;
    private readonly AppConfigOption _appConfigOption = appConfigOptions.Value;

    public CacheServiceTypes ServiceType => CacheServiceTypes.InMemory;

    public async Task<bool> ContainsAsync(string key, CancellationToken cancellation = default)
    {
        await Task.CompletedTask;
        return _memoryCache.TryGetValue(key, out _);
    }

    public async Task<T> GetAsync<T>(string key, CancellationToken cancellation = default)
    {
        await Task.CompletedTask;
        return  _memoryCache.Get<T>(key);
    }

    public async Task RemoveAsync(string key, CancellationToken cancellation = default)
    {
        await Task.CompletedTask;
        _memoryCache.Remove(key);
    }

    public async Task SetAsync<T>(string key, T value, int? expirationTime = null, CancellationToken cancellation = default)
    {
        await Task.CompletedTask;
        _memoryCache.Set(key, value, new MemoryCacheEntryOptions
        {
            SlidingExpiration = TimeSpan.FromSeconds(45),
            AbsoluteExpirationRelativeToNow = expirationTime.HasValue
                    ? TimeSpan.FromSeconds(expirationTime.Value)
                    : TimeSpan.FromSeconds(_appConfigOption.CacheExpiration)
        });
    }

    public Task<T> UpdateAsync<T>(string key, T data)
    {
        throw new NotImplementedException();
    }
}
