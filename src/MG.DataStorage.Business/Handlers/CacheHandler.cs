using System.Text.Json;
using MG.DataStorage.Core.DTOs;
using MG.DataStorage.Core.Interfaces;

namespace MG.DataStorage.Business.Handlers;

public sealed class CacheHandler : DataHandler
{
    private readonly IDataService _dataService;

    public CacheHandler(IDataService dataService)
    {
        _dataService = dataService;
    }

    protected override DataSource SourceType => DataSource.Cache;

    protected override async Task<JsonElement?> FetchContent(string id, CancellationToken cancellationToken = default)
    {
        return await _dataService.GetByIdAsync(id, cancellationToken);
    }

    protected override async Task PostFetch(DataRetrievalResult data, CancellationToken cancellationToken = default)
    {
        //fire and forget. we don't really need to wait for saving to any level of cache and return response fsater
        _ = Task.Run(async () =>
        {
            try
            {
                await _dataService.SetAsync(data.Id, data.Payload, CancellationToken.None);
            }
            catch
            {
                // TODO fails need to be registered
            }
        }, cancellationToken);
    }
}