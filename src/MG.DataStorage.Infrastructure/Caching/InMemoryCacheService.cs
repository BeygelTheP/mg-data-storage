using System.Collections.Concurrent;
using MG.DataStorage.Core.DTOs;
using MG.DataStorage.Core.Interfaces;
using MG.DataStorage.Infrastructure.Configuration;
using Microsoft.Extensions.Options;

namespace MG.DataStorage.Infrastructure.Caching;

public class InMemoryCacheService : ICacheService
{
    
    private readonly ConcurrentDictionary<string, (string data, TimeSpan ttl)> _cache;
    private readonly long _maxSizeBytes;

    private long _currentSize;

    public DataSource SourceType => DataSource.Cache;

    public InMemoryCacheService(IOptions<CacheSettings> options)
    {
        _cache = [];
        _maxSizeBytes = options.Value.InMemoryMaxSizeMB * 1024 * 1024;

    }

    public Task<string?> GetByIdAsync(string id, CancellationToken ct = default)
    {        
        return Task.FromResult($"mock string from {nameof(InMemoryCacheService)}");
    }

    public Task SetAsync(string id, string data, TimeSpan ttl, CancellationToken ct = default)
    {        
        return Task.CompletedTask;
    }

    public Task<bool> IsFullAsync(CancellationToken ct = default)
        => Task.FromResult(_currentSize > _maxSizeBytes);

    public Task SetAsync(string id, string content, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
