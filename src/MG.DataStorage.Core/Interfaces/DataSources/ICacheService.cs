namespace MG.DataStorage.Core.Interfaces;

public interface ICacheService : IDataService
{
    Task<bool> IsFullAsync(CancellationToken cancellationToken = default);
}