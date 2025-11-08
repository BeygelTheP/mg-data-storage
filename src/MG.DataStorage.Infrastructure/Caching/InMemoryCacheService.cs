using System.Collections.Concurrent;
using System.Text.Json;
using MG.DataStorage.Core.DTOs;
using MG.DataStorage.Core.Interfaces;
using MG.DataStorage.Infrastructure.Configuration;
using Microsoft.Extensions.Options;

namespace MG.DataStorage.Infrastructure.Caching;

public class InMemoryCacheService : ICacheService
{
    private readonly ConcurrentDictionary<string, CacheItem> _cache;
    private readonly CacheSettings _settings;
    private readonly long _maxSizeBytes;
    private long _currentSize;

    public DataSource SourceType => DataSource.Cache;

    public InMemoryCacheService(IOptions<CacheSettings> options)
    {
        _settings = options.Value;
        _cache = [];
        _maxSizeBytes = _settings.InMemoryMaxSizeMB * 1024 * 1024;
    }

    public bool IsFull() => _currentSize > _maxSizeBytes;

    public Task<JsonElement?> GetByIdAsync(string id, CancellationToken ct = default)
    {
        if (_cache.TryGetValue(id, out var cacheItem))
        {
            if (DateTime.UtcNow < cacheItem.ExpiresAt)
            {
                return Task.FromResult<JsonElement?>(cacheItem.Data);
            }
            else
            {
                // Remove expired item atomically
                if (_cache.TryRemove(id, out var removedItem))
                {
                    Interlocked.Add(ref _currentSize, -removedItem.Size);
                }
            }
        }
        
        return Task.FromResult<JsonElement?>(null);
    }

    public Task SetAsync(string id, JsonElement content, CancellationToken cancellationToken = default)
    {
        var expiresAt = DateTime.UtcNow.AddMinutes(_settings.CacheDurationMinutes);
        var jsonString = JsonSerializer.Serialize(content);
        var dataSize = System.Text.Encoding.UTF8.GetByteCount(jsonString);
        
        var newItem = new CacheItem
        {
            Data = content,
            ExpiresAt = expiresAt,
            Size = dataSize
        };

        _cache.AddOrUpdate(id, 
            // Add factory - called only for new keys
            key => 
            {
                Interlocked.Add(ref _currentSize, dataSize);
                return newItem;
            },
            // Update factory - called only for existing keys  
            (key, oldValue) => 
            {
                Interlocked.Add(ref _currentSize, dataSize - oldValue.Size);
                return newItem;
            });
        
        return Task.CompletedTask;
    }

    private class CacheItem
    {
        public required JsonElement Data { get; set; }
        public required DateTime ExpiresAt { get; set; }
        public required long Size { get; set; }
    }
}
