namespace IdentityWebApp.Api.Services;

/// <summary>
/// Сервис работы с константами API.
/// </summary>
internal static class ConstantsService
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
    /// Ошибка: неверные учётные данные.
    /// </summary>
    public const string ErrorInvalidCredentials = "Неверный логин или пароль.";

     /// <summary>
    /// Ошибка: некорректный адрес сервера (ошибка формата URI).
    /// </summary>
    public const string ErrorInvalidServerAddress = "Некорректный адрес сервера. Проверьте имя сервера и порт.";

    /// <summary>
    /// Ошибка: ошибка подключения к серверу.
    /// </summary>
    public const string ErrorConnectionFailed = "Ошибка подключения к серверу.";

    /// <summary>
    /// Ошибка: ошибка сервера с указанием статуса и содержимого.
    /// </summary>
    public const string ErrorServerError = "Ошибка сервера: {0} - {1}.";

    /// <summary>
    /// Ошибка: ошибка десериализации токена.
    /// </summary>
    public const string ErrorTokenDeserializationFailed = "Ошибка десериализации токена.";

    /// <summary>
    /// Ошибка: аутентификация отменена.
    /// </summary>
    public const string ErrorAuthenticationCancelled = "Аутентификация отменена.";

     /// <summary>
    /// Ошибка: непредвиденная ошибка при аутентификации.
    /// </summary>
    public const string ErrorUnexpectedAuthenticationError = "Произошла непредвиденная ошибка при аутентификации.";

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
}
