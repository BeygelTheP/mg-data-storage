using MG.DataStorage.Core.DTOs;
using MG.DataStorage.Core.Interfaces;

namespace MG.DataStorage.Business.Handlers;

public sealed class CacheHandler : DataHandler
{
    private readonly ICacheService _cacheService;

    public CacheHandler(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    public override async Task<DataRetrievalResult?> HandleAsync(string id, CancellationToken cancellationToken = default)
    {
        return new DataRetrievalResult
        {
            Content = "Mock text",
            Id = id,
            RetrievedFrom = DataSource.Cache,            
        };        
    }
}