namespace IdentityWebApp.Api.Models;

/// <summary>
/// Модель параметров подключения к серверу для аутентификации пользователей через API.
/// </summary>
public class ConnectionOptionsModel
{
    /// <summary>
    /// Получает или задаёт имя сервера, к которому будет осуществляться подключение.
    /// </summary>
    public required string ServerName { get; set; }

    /// <summary>
    /// Получает или задаёт номер порта для подключения к серверу. Может быть null, если порт не указан.
    /// </summary>
    public int? Port { get; set; }

    /// <summary>
    /// Получает или задаёт логин пользователя для аутентификации.
    /// </summary>
    public required string Username { get; set; }

    /// <summary>
    /// Пароль пользователя для аутентификации.
    /// </summary>
    public required string Password { get; set; }
}
