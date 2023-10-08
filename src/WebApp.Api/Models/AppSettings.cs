namespace WebApp.Api.Models;

public class AppSettings
{
    public ConnectionStrings? ConnectionStrings { get; set; }
    public ApplicationOptions? AppOptions { get; set; }
    public JWTTokensOptions? JWT { get; set; }
    public LoggingOptions? Logging { get; set; }
}
public class JWTTokensOptions
{
    public string? Audience { get; set; }
    //[JsonPropertyName("issuer")]
    public string? Issuer { get; set; }
    public string? AccessTokenExpiration { get; set; }
    public string? Secret { get; set; }
    public string? RefreshTokenExpiration { get; set; }        
    //public bool AllowMultipleLoginsFromTheSameUser { set; get; }
    //public bool AllowSignoutAllUserActiveClients { set; get; }
}

public class ApplicationOptions
{
    public string? LoginPath { get; set; }
    public string? LogoutPath { get; set; }
    public string? AdminRoleName { get; set; }
    public string? UrlApi { get; set; }
}

public class ConnectionStrings
{
    public string? DefaultConnection { get; set; }
    public string? ConnectionPG { get; set; }
    public string? SqlLiteConnection { get; set; }
}

public class LoggingOptions
{
    public Dictionary<string, string>? LogLevel { get; set; }
}