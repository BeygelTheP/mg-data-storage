using System.Text.Json;
using MG.DataStorage.Core.DTOs;
using MG.DataStorage.Core.Interfaces;
using MG.DataStorage.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace MG.DataStorage.Infrastructure.Caching;

public sealed class RedisCacheService : ICacheService, IDisposable
{
    private readonly IConnectionMultiplexer _redis;
    private readonly IDatabase _database;
    private readonly CacheSettings _settings;
    private readonly SemaphoreSlim _connectionSemaphore;

    public DataSource SourceType => DataSource.Cache;

    public RedisCacheService(IOptions<CacheSettings> settings)
    {
        _settings = settings.Value;
        _connectionSemaphore = new SemaphoreSlim(1, 1);
        
        if (string.IsNullOrEmpty(_settings.RedisConnectionString))
        {
            throw new InvalidOperationException("Redis connection string is not configured");
        }

        var options = ConfigurationOptions.Parse(_settings.RedisConnectionString);
        options.AbortOnConnectFail = false; // Allow retries
        options.ConnectRetry = 3;
        options.ConnectTimeout = 5000;
        
        _redis = ConnectionMultiplexer.Connect(options);
        _database = _redis.GetDatabase();
    }

    public bool IsFull() => false; // Redis doesn't have memory limits in our implementation

    public async Task<JsonElement?> GetByIdAsync(string key, CancellationToken ct = default)
    {
        try
        {
            await _connectionSemaphore.WaitAsync(ct);
            
            var value = await _database.StringGetAsync(key);
            if (!value.HasValue)
                return null;

            var jsonString = value.ToString();
            return JsonSerializer.Deserialize<JsonElement>(jsonString);
        }
        catch (RedisConnectionException)
        {
            // Redis is unavailable, silently fail to not break obviously it should be handled by logging etc.
            return null;
        }
        finally
        {
            _connectionSemaphore.Release();
        }
    }

    public async Task SetAsync(string id, JsonElement content, CancellationToken cancellationToken = default)
    {
        try
        {
            await _connectionSemaphore.WaitAsync(cancellationToken);
            
            var jsonString = JsonSerializer.Serialize(content);
            var expiry = TimeSpan.FromMinutes(_settings.CacheDurationMinutes);
            
            await _database.StringSetAsync(id, jsonString, expiry);
        }
        catch (RedisConnectionException)
        {
            // Redis is unavailable, silently fail to not break obviously it should be handled by logging etc.
            return;
        }
        finally
        {
            _connectionSemaphore.Release();
        }
    }

    public void Dispose()
    {
        _connectionSemaphore?.Dispose();
        _redis?.Dispose();
    }
}