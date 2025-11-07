using MG.DataStorage.Core.DTOs;
using MG.DataStorage.Core.Interfaces;

namespace MG.DataStorage.Infrastructure.Caching;

public sealed class RedisCacheService : ICacheService
{
    public DataSource SourceType => DataSource.Cache;

    public RedisCacheService()
    {
       
    }

    public async Task<string?> GetByIdAsync(string key, CancellationToken ct = default)
    {        
        return $"Mock string from {nameof(GetByIdAsync)}";
    }

    public async Task SetAsync(string key, string value, TimeSpan ttl, CancellationToken ct = default)
    {

    }

    public Task<bool> IsFullAsync(CancellationToken ct = default)
        => Task.FromResult(false); // Redis usually not limited here

    public Task SetAsync(string id, string content, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}