namespace MG.DataStorage.Core.Interfaces;

public interface IDataRepository
{
    Task<string?> GetByIdAsync(string id, CancellationToken cancellationToken = default);

    Task SaveAsync(string id, string content, CancellationToken cancellationToken = default);
    
    Task<bool> ExistsAsync(string id, CancellationToken cancellationToken = default);
}