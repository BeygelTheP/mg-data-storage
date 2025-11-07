namespace MG.DataStorage.Infrastructure.Configuration;

public sealed class FileStorageSettings
{    
    public const string CONFIG_NAME = "FileStorage";

    public string BasePath { get; set; } = "./data";
    
    public int FileCacheDurationMinutes { get; set; } = 30;
}
