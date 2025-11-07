using MG.DataStorage.Core.DTOs;
using MG.DataStorage.Core.Interfaces;

namespace MG.DataStorage.Infrastructure.FileStorage;

public class FileStorageService : IFileStorageService
{
    public DataSource SourceType => DataSource.File;

    public Task<string?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task SetAsync(string id, string content, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}