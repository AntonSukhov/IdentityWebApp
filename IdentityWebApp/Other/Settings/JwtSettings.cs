namespace IdentityWebApp.Other.Settings;

/// <summary>
/// Настройки JWT-токена.
/// </summary>
public class JwtSettings
{
   #region  Свойства

   /// <summary>
   /// Получает или задает значение времени истечения действия токена в секундах. По умолчанию 10 минут.
   /// </summary>
   public int ExpiresInSeconds { get; set; } = 600;

   #endregion
}
