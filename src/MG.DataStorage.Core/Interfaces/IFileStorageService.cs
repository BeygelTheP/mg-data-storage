namespace MG.DataStorage.Core.Interfaces;

public interface IFileStorageService
{
    Task<string?> GetAsync(string id, CancellationToken cancellationToken = default);

    Task SetAsync(string id, string content, TimeSpan ttl, CancellationToken cancellationToken = default);

    Task RemoveAsync(string id, CancellationToken cancellationToken = default);

    Task CleanupExpiredFilesAsync(CancellationToken cancellationToken = default);
}