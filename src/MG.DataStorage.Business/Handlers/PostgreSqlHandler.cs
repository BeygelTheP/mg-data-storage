using System.Text.Json;
using MG.DataStorage.Core.DTOs;
using MG.DataStorage.Core.Interfaces;

namespace MG.DataStorage.Business.Handlers;

public sealed class PostgreSqlHandler : DataHandler
{
    private readonly IDataService _dataService;

    public PostgreSqlHandler(IDataService dataService)
    {
        _dataService = dataService;
    }

    protected override DataSource SourceType => DataSource.Database;

    protected override async Task<JsonElement?> FetchContent(string id, CancellationToken cancellationToken = default)
    {
        return await _dataService.GetByIdAsync(id, cancellationToken);
    }
}