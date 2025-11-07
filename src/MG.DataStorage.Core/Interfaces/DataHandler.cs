using MG.DataStorage.Core.DTOs;

namespace MG.DataStorage.Core.Interfaces;

public abstract class DataHandler
{
    protected abstract DataSource SourceType { get; }
    protected DataHandler? _nextHandler;

    public DataHandler SetNext(DataHandler handler)
    {
        _nextHandler = handler;
        return handler;
    }
    protected abstract Task<string?> FetchContent(string id, CancellationToken cancellationToken = default);
    protected virtual async Task PostFetch(DataRetrievalResult data, CancellationToken cancellationToken = default) { }
    public virtual async Task<DataRetrievalResult?> HandleAsync(string id, CancellationToken cancellationToken = default)
    {
        var payload = await FetchContent(id, cancellationToken);
        if (payload is not null)
            return new DataRetrievalResult
            {
                Payload = payload,
                Id = id,
                RetrievedFrom = SourceType,
            };

        var result = _nextHandler is not null
            ? await _nextHandler.HandleAsync(id, cancellationToken)
            : null;
        if (result is not null)
        {
            await PostFetch(result, cancellationToken);
        }
        return result;
    }
}

