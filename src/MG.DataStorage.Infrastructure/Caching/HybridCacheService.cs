using System.Text.Json;
using MG.DataStorage.Core.DTOs;
using MG.DataStorage.Core.Interfaces;

namespace MG.DataStorage.Infrastructure.Caching;

public class HybridCacheService : ICacheService
{
    private readonly InMemoryCacheService _memory;
    private readonly RedisCacheService _redis;

    public DataSource SourceType => DataSource.Cache;

    public HybridCacheService(InMemoryCacheService memory, RedisCacheService redis)
    {
        _memory = memory;
        _redis = redis;
    }

    public async Task<JsonElement?> GetByIdAsync(string key, CancellationToken ct = default)
    {
        var payload = await _memory.GetByIdAsync(key, ct);
        if (payload != null) return payload;

        payload = await _redis.GetByIdAsync(key, ct);
        if (payload.HasValue)
            await _memory.SetAsync(key, payload.Value, ct);
        return payload;
    }

    public bool IsFull() => false; // Hybrid cache can always accept data (memory or Redis)

    public async Task SetAsync(string id, JsonElement content, CancellationToken cancellationToken = default)
    {
        if (!_memory.IsFull())
            await _memory.SetAsync(id, content, cancellationToken);
        await _redis.SetAsync(id, content, cancellationToken);
    }
}
