using System.Net.Mail;

namespace IdentityWebApp.Other;

/// <summary>
/// Настройки протокола SMTP.
/// </summary>
public class SmtpSettings
{
    #region Свойства

    /// <summary>
    /// Получает или задаёт значение хоста.
    /// </summary>
    public required string Host { get; set; }

    /// <summary>
    /// Получает или задаёт значение порта.
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    /// Получает или задаёт значение имени пользователя.
    /// </summary>
    public required string Username { get; set; }

    /// <summary>
    /// Получает или задаёт метод доставки электронных писем.
    /// </summary>
    public SmtpDeliveryMethod DeliveryMethod { get; set; } = SmtpDeliveryMethod.Network;

    /// <summary>
    /// Получает или задаёт значение признака использовать или нет 
    /// учётные данные текущего вошедшего пользователя при выполнении запросов.
    /// </summary>
    public bool UseDefaultCredentials { get; set; }

    #endregion
}
