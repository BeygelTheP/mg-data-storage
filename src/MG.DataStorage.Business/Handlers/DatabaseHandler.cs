using MG.DataStorage.Core.DTOs;
using MG.DataStorage.Core.Interfaces;

namespace MG.DataStorage.Business.Handlers;

public sealed class DatabaseHandler : DataHandler
{
    private IDataRepository repoService;

    public DatabaseHandler(IDataRepository repoService)
    {
        this.repoService = repoService;
    }

    public override async Task<DataRetrievalResult?> HandleAsync(string id, CancellationToken cancellationToken = default)
    {
        return new DataRetrievalResult
        {
            Content = "Mock text",
            Id = id,
            RetrievedFrom = DataSource.Database,
        };
    }
}