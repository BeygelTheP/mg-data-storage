using MG.DataStorage.Core.DTOs;
using MG.DataStorage.Core.Interfaces;

namespace MG.DataStorage.Business.Services;

public class DataRetrievalService : IDataRetrievalService
{
    private readonly DataHandler _handler;

    public DataRetrievalService(DataHandler handler)
    {
        _handler = handler;
    }

    public async Task<DataRetrievalResult?> GetDataAsync(string id, CancellationToken cancellationToken = default)
        => await _handler.HandleAsync(id, cancellationToken);
}