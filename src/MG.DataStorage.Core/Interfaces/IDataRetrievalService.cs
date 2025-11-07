using MG.DataStorage.Core.DTOs;

namespace MG.DataStorage.Core.Interfaces;

public interface IDataRetrievalService
{
    Task<DataRetrievalResult?> GetDataAsync(string id, CancellationToken cancellationToken = default);
}