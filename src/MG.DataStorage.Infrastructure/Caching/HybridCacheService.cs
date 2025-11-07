using MG.DataStorage.Core.Interfaces;

namespace MG.DataStorage.Infrastructure.Caching;

public class HybridCacheService : ICacheService
{
    private readonly InMemoryCacheService _memory;
    private readonly RedisCacheService _redis;

    public HybridCacheService(InMemoryCacheService memory, RedisCacheService redis)
    {
        _memory = memory;
        _redis = redis;
    }

    public async Task<string?> GetByIdAsync(string key, CancellationToken ct = default)
    {
        var value = await _memory.GetByIdAsync(key, ct);
        if (value != null) return value;

        value = await _redis.GetByIdAsync(key, ct);
        if (value != null)
            await _memory.SetAsync(key, value, TimeSpan.FromMinutes(10), ct);
        return value;
    }

    public async Task SetAsync(string key, string value, TimeSpan ttl, CancellationToken ct = default)
    {
        if (await _memory.IsFullAsync(ct))
            await _redis.SetAsync(key, value, ttl, ct);
        else
            await _memory.SetAsync(key, value, ttl, ct);
    }

    public Task<bool> IsFullAsync(CancellationToken ct = default)
        => _memory.IsFullAsync(ct);
}
