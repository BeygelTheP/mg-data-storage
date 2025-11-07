using System.Text.Json;
using MG.DataStorage.Core.DTOs;
using MG.DataStorage.Core.Interfaces;

namespace MG.DataStorage.Infrastructure.Caching;

public sealed class RedisCacheService : ICacheService
{
    public DataSource SourceType => DataSource.Cache;

    public RedisCacheService()
    {

    }
    public Task<bool> IsFullAsync(CancellationToken ct = default) => Task.FromResult(false);

    public Task<JsonElement?> GetByIdAsync(string key, CancellationToken ct = default)
    {
        //todo implement
        return Task.FromResult<JsonElement?>(null);
    }

    public Task SetAsync(string id, JsonElement content, CancellationToken cancellationToken = default)
    {
        //todo implement
        return Task.CompletedTask;
    }
}