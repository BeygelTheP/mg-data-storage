using System.Text.Json.Serialization;
namespace MG.DataStorage.Core.DTOs;

public class DataRetrievalResult
{
    public required string Id { get; init; }

    public required string Payload { get; init; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required DataSource RetrievedFrom { get; init; }

    public DateTime RetrievedAtUtc { get; init; } = DateTime.UtcNow;
}

public enum DataSource
{
    Cache,
    File,
    Database
}