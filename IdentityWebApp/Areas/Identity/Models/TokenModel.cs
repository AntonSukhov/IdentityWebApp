using System.Text.Json.Serialization;

namespace IdentityWebApp.Areas.Identity.Models;

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
    public DateTime Expires { get; set; }

    #endregion
}
