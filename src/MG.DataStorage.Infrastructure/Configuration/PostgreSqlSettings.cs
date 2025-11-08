namespace MG.DataStorage.Infrastructure.Configuration;

public class PostgreSqlSettings
{
    public const string CONFIG_NAME = "PostgreSqlSettings";

    public string ConnectionString { get; set; } = string.Empty;
    public string TableName { get; set; } = "data_storage";
    public int CommandTimeoutSeconds { get; set; } = 30;
}