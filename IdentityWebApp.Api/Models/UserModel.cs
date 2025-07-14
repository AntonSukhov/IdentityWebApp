namespace IdentityWebApp.Api.Models;

/// <summary>
/// Модель для входа пользователя системы.
/// </summary>
public class UserModel
{
    #region Свойства

    /// <summary>
    /// Получает или задаёт логин пользователя для аутентификации.
    /// </summary>
    public required string Login { get; set; }
    /// <summary>
    /// Получает или задаёт пароль пользователя для аутентификации.
    /// </summary>
    public required string Password { get; set; }

    #endregion
}
