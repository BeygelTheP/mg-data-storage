using MG.DataStorage.Core.DTOs;
using MG.DataStorage.Core.Interfaces;

namespace MG.DataStorage.Infrastructure.Repositories;

public class DatabaseRepository : IDataRepository
{
    public DataSource SourceType => DataSource.Database;

    public Task<string?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task SetAsync(string id, string content, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}