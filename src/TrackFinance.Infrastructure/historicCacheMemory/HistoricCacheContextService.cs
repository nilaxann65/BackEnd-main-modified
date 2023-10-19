using Microsoft.Extensions.Caching.Memory;
using System;
using TrackFinance.Core.Interfaces;

namespace TrackFinance.Infrastructure.cacheMemory;

public class HistoricCacheContextService: IHistoricCacheContextService
{
    private readonly IMemoryCache _cache;
    private static readonly string _name = "historic";

    public HistoricCacheContextService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public bool existCache(string userId)
    {
        return _cache.TryGetValue(buildKey(userId), out _);
    }

    public void setCache(string userId, object value)
    {
        removeCache(userId);
        _cache.Set(buildKey(userId), value, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
        });
    }

    public object getCache(string userId)
    {
        return _cache.Get(buildKey(userId));
    }

    private void removeCache(string userId)
    {
        _cache.Remove(buildKey(userId));
    }

    private static string buildKey(string userId)
    {
        return _name + '-' + userId;
    }
}
