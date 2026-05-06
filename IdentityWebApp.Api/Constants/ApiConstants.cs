namespace IdentityWebApp.Api.Constants;

/// <summary>
/// Константы API.
/// </summary>
internal static class ApiConstants
{
    public const string LoginUri = "/api/token-auth/login";
    
    /// <summary>
    /// Порт по умолчанию для HTTPS.
    /// </summary>
    public const int DefaultHttpsPort = 443;

    /// <summary>
    /// Порт по умолчанию для HTTP.
    /// </summary>
    public const int DefaultHttpPort = 80;

    /// <summary>
    /// Таймаут HTTP‑клиента по умолчанию (в секундах).
    /// </summary>
    public const int DefaultHttpClientTimeoutSeconds = 30;

    /// <summary>
    /// Схема для API (HTTPS).
    /// </summary>
    public const string HttpsApiScheme = "https";

    /// <summary>
    /// Схема для API (HTTP).
    /// </summary>
    public const string HttpApiScheme = "http";

    /// <summary>
    /// Название http-клиента.
    /// </summary>
    public const string HttpClientName = "IdentityWebApi";

    /// <summary>
    /// Название секции Аутентификации.
    /// </summary>
    public const string AuthenticationSectionName = "Authentication";
}
