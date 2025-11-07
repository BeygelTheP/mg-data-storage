using MG.DataStorage.Core.DTOs;

namespace MG.DataStorage.Core.Interfaces;

public interface IDataProviderFactory
{
    IDataService CreateDataSource(DataSource sourceType);    
}
