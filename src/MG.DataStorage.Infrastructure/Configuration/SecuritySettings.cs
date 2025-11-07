namespace MG.DataStorage.Infrastructure.Configuration;

public sealed class SecuritySettings
{
    public const string CONFIG_NAME = "Security";
    public string ApiKey { get; set; } = string.Empty;
    public RateLimitSettings RateLimit { get; set; } = new();
}


public sealed class RateLimitSettings
{
    public bool Enabled { get; set; } = true;
    public int PermitLimit { get; set; } = 100;
    public int WindowMinutes { get; set; } = 1;
}