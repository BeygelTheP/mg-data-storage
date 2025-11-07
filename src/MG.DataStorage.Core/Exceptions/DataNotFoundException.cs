namespace MG.DataStorage.Core.Exceptions;

public class DataNotFoundException : Exception
{
    public string DataId { get; }

    public DataNotFoundException(string dataId)
        : base($"Data with ID '{dataId}' was not found in any storage layer.")
    {
        DataId = dataId;
    }

    public DataNotFoundException(string dataId, string message)
        : base(message)
    {
        DataId = dataId;
    }

    public DataNotFoundException(string dataId, string message, Exception innerException)
        : base(message, innerException)
    {
        DataId = dataId;
    }
}
