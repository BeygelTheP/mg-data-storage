namespace MG.DataStorage.Core.Interfaces;

public interface ICacheService
{
    Task<string?> GetByIdAsync(string id, CancellationToken ct = default);
    Task SetAsync(string key, string value, TimeSpan ttl, CancellationToken ct = default);
    Task<bool> IsFullAsync(CancellationToken ct = default); // for hybrid strategy
}