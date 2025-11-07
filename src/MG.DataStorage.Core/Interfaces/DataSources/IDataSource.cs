using System.Text.Json;
using MG.DataStorage.Core.DTOs;

namespace MG.DataStorage.Core.Interfaces;

public interface IDataService
{
    DataSource SourceType { get; }

    Task<JsonElement?> GetByIdAsync(string id, CancellationToken cancellationToken = default);

    Task SetAsync(string id, JsonElement content, CancellationToken cancellationToken = default);
}