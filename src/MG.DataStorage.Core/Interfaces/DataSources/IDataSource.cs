using MG.DataStorage.Core.DTOs;

namespace MG.DataStorage.Core.Interfaces;

public interface IDataService
{
    DataSource SourceType { get; }

    Task<string?> GetByIdAsync(string id, CancellationToken cancellationToken = default);

    Task SetAsync(string id, string content, CancellationToken cancellationToken = default);
}