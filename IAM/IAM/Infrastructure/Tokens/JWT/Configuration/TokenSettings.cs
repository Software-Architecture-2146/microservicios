namespace Frock_backend.IAM.Infrastructure.Tokens.JWT.Configuration;

public class TokenSettings
{
    public string Secret { get; set; } = string.Empty;
    public int ExpirationInDays { get; set; } = 7;
    public string Issuer { get; set; } = "FrockIAM";
    public string Audience { get; set; } = "FrockUsers";
}