using System.Globalization;
using System.Text.Json;
using MG.DataStorage.Core.DTOs;
using MG.DataStorage.Core.Interfaces;
using MG.DataStorage.Infrastructure.Configuration;
using Microsoft.Extensions.Options;

namespace MG.DataStorage.Infrastructure.FileStorage;

public class FileStorageService : IFileStorageService
{
    private readonly FileStorageSettings _settings;

    public FileStorageService(IOptions<FileStorageSettings> settings)
    {
        _settings = settings.Value;
        
        // Ensure base directory exists
        if (!Directory.Exists(_settings.BasePath))
        {
            Directory.CreateDirectory(_settings.BasePath);
        }
    }

    public DataSource SourceType => DataSource.File;

    public async Task<JsonElement?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var files = Directory.GetFiles(_settings.BasePath, $"{id}_*.json");
        
        foreach (var file in files)
        {
            var fileName = Path.GetFileName(file);
            if (IsFileExpired(fileName))
            {
                // Clean up expired file
                File.Delete(file);
                continue;
            }
            
            // File is still valid, read and return content
            var jsonContent = await File.ReadAllTextAsync(file, cancellationToken);
            return JsonSerializer.Deserialize<JsonElement>(jsonContent);
        }
        
        return null;
    }

    public async Task SetAsync(string id, JsonElement content, CancellationToken cancellationToken = default)
    {
        // Clean up any existing files for this id
        var existingFiles = Directory.GetFiles(_settings.BasePath, $"{id}_*.json");
        foreach (var file in existingFiles)
        {
            File.Delete(file);
        }
        
        // Create new file with expiration timestamp in filename
        var expirationTime = DateTime.UtcNow.AddMinutes(_settings.FileCacheDurationMinutes);
        var timestamp = expirationTime.ToString("yyyyMMdd_HHmmss");
        var fileName = $"{id}_{timestamp}.json";
        var filePath = Path.Combine(_settings.BasePath, fileName);
        
        var jsonContent = JsonSerializer.Serialize(content, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(filePath, jsonContent, cancellationToken);
    }

    private bool IsFileExpired(string fileName)
    {
        // Extract timestamp from filename format: id_yyyyMMdd_HHmmss.json
        var parts = fileName.Replace(".json", "").Split('_');
        if (parts.Length < 3) return true;
        
        var datePart = parts[^2]; // Second to last part
        var timePart = parts[^1]; // Last part
        
        if (DateTime.TryParseExact($"{datePart}_{timePart}", "yyyyMMdd_HHmmss", null, 
            DateTimeStyles.None, out var expirationTime))
        {
            return DateTime.UtcNow > expirationTime;
        }
        
        return true; // If we can't parse, consider it expired
    }
}