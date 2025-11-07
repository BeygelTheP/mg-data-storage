using MG.DataStorage.Core.DTOs;
using MG.DataStorage.Core.Interfaces;
using MG.DataStorage.Infrastructure.Caching;
using MG.DataStorage.Infrastructure.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
public class DataProviderFactory : IDataProviderFactory
{
    private readonly IServiceProvider _sp;
    private readonly CacheSettings _config;
    public DataProviderFactory(IServiceProvider sp, IOptions<CacheSettings> config)
    {
        _config = config.Value;
        _sp = sp;
    }

    private ICacheService CreateCacheService()
    {
        var mode = _config.Strategy.ToLowerInvariant();

        return mode switch
        {
            "inmemory" => _sp.GetRequiredService<InMemoryCacheService>(),
            "redis" => _sp.GetRequiredService<RedisCacheService>(),
            "hybrid" => _sp.GetRequiredService<HybridCacheService>(),
            _ => _sp.GetRequiredService<InMemoryCacheService>()
        };
    }
    public IDataService CreateDataSource(DataSource sourceType)
    {
        return sourceType switch
        {
            DataSource.Cache => CreateCacheService(),
            DataSource.File => _sp.GetRequiredService<IFileStorageService>(),
            DataSource.Database => _sp.GetRequiredService<IDataRepository>(),
            _ => throw new ArgumentOutOfRangeException(nameof(sourceType), sourceType, null),
        };
    }
}