using MG.DataStorage.Core.Interfaces;

namespace MG.DataStorage.Infrastructure.FileStorage;

public class FileStorageService : IFileStorageService
{
    public Task<string?> GetAsync(string id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException("TODO: Implement file retrieval with expiration check");
    }

    public Task SetAsync(string id, string content, TimeSpan ttl, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException("TODO: Implement file storage with expiration timestamp in filename");
    }

    public Task RemoveAsync(string id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException("TODO: Implement file removal");
    }

    public Task CleanupExpiredFilesAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException("TODO: Implement cleanup of expired files");
    }
}