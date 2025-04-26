namespace IdentityWebApp.Other.Settings;

/// <summary>
/// Настройки JWT-токена.
/// </summary>
public class JwtSettings
{
   #region  Свойства

   /// <summary>
   /// Получает или задает значение времени истечения действия токена в минутах. По умолчанию 10.
   /// </summary>
   public int ExpiresInMinutes { get; set; } = 10;

   #endregion
}
