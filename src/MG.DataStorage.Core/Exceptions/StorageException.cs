namespace MG.DataStorage.Core.Exceptions;

public class StorageException : Exception
{
    public string? StorageType { get; }

    public StorageException(string message)
        : base(message)
    {
    }

    public StorageException(string message, string storageType)
        : base(message)
    {
        StorageType = storageType;
    }

    public StorageException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public StorageException(string message, string storageType, Exception innerException)
        : base(message, innerException)
    {
        StorageType = storageType;
    }
}