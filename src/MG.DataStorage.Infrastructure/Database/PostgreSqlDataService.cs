using System.Text.Json;
using MG.DataStorage.Core.DTOs;
using MG.DataStorage.Core.Interfaces;
using MG.DataStorage.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using Npgsql;
using NpgsqlTypes;

namespace MG.DataStorage.Infrastructure.Database;

public class PostgreSqlDataService : IDataRepository
{
    private readonly PostgreSqlSettings _settings;

    public DataSource SourceType => DataSource.Database;

    public PostgreSqlDataService(IOptions<PostgreSqlSettings> settings)
    {
        _settings = settings.Value;
    }

    public async Task<JsonElement?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        using var connection = new NpgsqlConnection(_settings.ConnectionString);
        await connection.OpenAsync(cancellationToken);

        using var command = new NpgsqlCommand(
            $"SELECT content FROM {_settings.TableName} WHERE id = @id",
            connection);

        command.Parameters.AddWithValue("@id", id);
        command.CommandTimeout = _settings.CommandTimeoutSeconds;

        var result = await command.ExecuteScalarAsync(cancellationToken);
        if (result?.ToString() is string jsonString)
        {
            return JsonSerializer.Deserialize<JsonElement>(jsonString);
        }
        return null;
    }

    public async Task SetAsync(string id, JsonElement content, CancellationToken cancellationToken = default)
    {
        using var connection = new NpgsqlConnection(_settings.ConnectionString);
        await connection.OpenAsync(cancellationToken);

        using var command = new NpgsqlCommand(
            $@"INSERT INTO {_settings.TableName} (id, content, created_at, updated_at) 
               VALUES (@id, @content, @now, @now) 
               ON CONFLICT (id) 
               DO UPDATE SET content = @content, updated_at = @now",
            connection);

        command.Parameters.AddWithValue("@id", id);
        command.Parameters.AddWithValue("@content", NpgsqlTypes.NpgsqlDbType.Jsonb, JsonSerializer.Serialize(content));
        command.Parameters.AddWithValue("@now", DateTime.UtcNow);
        command.CommandTimeout = _settings.CommandTimeoutSeconds;

        await command.ExecuteNonQueryAsync(cancellationToken);
    }

}