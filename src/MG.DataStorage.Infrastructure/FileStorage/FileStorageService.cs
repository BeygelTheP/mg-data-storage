using System.Text.Json;
using MG.DataStorage.Core.DTOs;
using MG.DataStorage.Core.Interfaces;

namespace MG.DataStorage.Infrastructure.FileStorage;

public class FileStorageService : IFileStorageService
{
    public DataSource SourceType => DataSource.File;

    public Task<JsonElement?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<JsonElement?>(null);
    }

    public Task SetAsync(string id, JsonElement content, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}