using MG.DataStorage.Core.Interfaces;

namespace MG.DataStorage.Infrastructure.Repositories;

public class DatabaseRepository : IDataRepository
{
    public Task<string?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException("TODO: Implement database retrieval - BLOCKED waiting for DB choice");
    }

    public Task SaveAsync(string id, string content, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException("TODO: Implement database save - BLOCKED waiting for DB choice");
    }

    public Task<bool> ExistsAsync(string id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException("TODO: Implement database exists check - BLOCKED waiting for DB choice");
    }
}