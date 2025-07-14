using System.Text.Json.Serialization;

namespace IdentityWebApp.Api.Models;

/// <summary>
/// Модель токена пользователя системы.
/// </summary>
public class TokenModel
{
    #region Свойства

    /// <summary>
    /// Получает или задаёт значение токена.
    /// </summary>
    [JsonPropertyName("token")]
    public required string Value { get; set; }

    /// <summary>
    /// Получает или задаёт значение даты и время истечения действия токена.
    /// </summary>
    [JsonPropertyName("expires")]
    public DateTimeOffset Expires { get; set; }

    #endregion
}
