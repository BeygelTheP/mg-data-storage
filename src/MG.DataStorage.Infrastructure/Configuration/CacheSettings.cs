namespace MG.DataStorage.Infrastructure.Configuration;

public sealed class CacheSettings
{
    public const string CONFIG_NAME = "CacheSettings";
    public string Strategy { get; set; } = "Hybrid";
    public uint InMemoryMaxSizeMB { get; set; } = 50;
    public uint CacheDurationMinutes { get; set; } = 10;
    public string? RedisConnectionString { get; set; }
    
}