namespace MG.DataStorage.Core.Interfaces;

public interface IDataProviderFactory
{
    ICacheService CreateCacheService();

    IFileStorageService CreateFileStorageService();

    IDataRepository CreateDataRepository();
}
