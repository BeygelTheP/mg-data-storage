using MG.DataStorage.Core.DTOs;
using MG.DataStorage.Core.Interfaces;

namespace MG.DataStorage.Business.Handlers;

public sealed class FileHandler : DataHandler
{
    private readonly IFileStorageService _fileService;

    public FileHandler(IFileStorageService fileService)
    {
        _fileService = fileService;
    }

    public override async Task<DataRetrievalResult?> HandleAsync(string id, CancellationToken cancellationToken = default)
    {
        return new DataRetrievalResult
        {
            Content = "Mock text",
            Id = id,
            RetrievedFrom = DataSource.File,            
        };
    }
}