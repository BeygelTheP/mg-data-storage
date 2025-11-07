using MG.DataStorage.Core.DTOs;

namespace MG.DataStorage.Core.Interfaces;

public abstract class DataHandler
{
    protected DataHandler? _nextHandler;

    public DataHandler SetNext(DataHandler handler)
    {
        _nextHandler = handler;
        return handler;
    }

    public abstract Task<DataRetrievalResult?> HandleAsync(string id, CancellationToken cancellationToken = default);
}

