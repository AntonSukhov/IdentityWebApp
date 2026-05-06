namespace IdentityWebApp.Api.Constants;

// <summary>
/// Константы сообщений об ошибках.
/// </summary>
internal static class ErrorMessagesConstants
{
    /// <summary>
    /// Ошибка: неверные учётные данные.
    /// </summary>
    public const string InvalidCredentials = "Неверный логин или пароль.";

    /// <summary>
    /// Ошибка: ошибка подключения к серверу.
    /// </summary>
    public const string ConnectionFailed = "Ошибка подключения к серверу.";

    /// <summary>
    /// Ошибка: ошибка сервера с указанием статуса и содержимого.
    /// </summary>
    public const string ServerError = "Ошибка сервера: {0} - {1}.";

    /// <summary>
    /// Ошибка: ошибка десериализации токена.
    /// </summary>
    public const string TokenDeserializationFailed = "Ошибка десериализации токена.";

    /// <summary>
    /// Ошибка: аутентификация отменена.
    /// </summary>
    public const string AuthenticationCancelled = "Аутентификация отменена.";

    /// <summary>
    /// Ошибка: непредвиденная ошибка при аутентификации.
    /// </summary>
    public const string UnexpectedAuthenticationError = "Произошла непредвиденная ошибка при аутентификации.";

    /// <summary>
    /// Ошибка: настройки аутентификации не настроены.
    /// </summary>
    public const string AuthenticationSettingsNotConfigured = "Настройки аутентификации не настроены.";

    /// <summary>
    /// Ошибка: ServerName в настройках аутентификации не может быть пустым.
    /// </summary>
    public const string ServerNameCannotBeEmpty =
        "Название сервера в настройках аутентификации не может быть null, пустым или содержать только пробелы.";

    /// <summary>
    /// Ошибка: неверный формат URI, сконструированного из настроек аутентификации.
    /// </summary>
    public const string InvalidUriFormat =
        "Неверный формат URI, сконструированный из настроек аутентификации. " +
        "ServerName: '{0}', Port: '{1}', Scheme: '{2}'.";
}

