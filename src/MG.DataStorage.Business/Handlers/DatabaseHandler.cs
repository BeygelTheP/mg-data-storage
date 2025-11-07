using MG.DataStorage.Core.DTOs;
using MG.DataStorage.Core.Interfaces;

namespace MG.DataStorage.Business.Handlers;

public sealed class DatabaseHandler : DataHandler
{
    private IDataService _dataService;

    public DatabaseHandler(IDataService dataService)
    {
        _dataService = dataService;
    }

    protected override DataSource SourceType => DataSource.Database;

    protected override async Task<string?> FetchContent(string id, CancellationToken cancellationToken = default)
    {
        return await _dataService.GetByIdAsync(id, cancellationToken);
    }
}