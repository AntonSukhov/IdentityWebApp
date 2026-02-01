namespace IdentityWebApp.Other.Settings;

/// <summary>
/// Настройки JWT-токена.
/// </summary>
public class JwtSettings
{
   /// <summary>
   /// Получает или задает значение времени истечения действия токена в секундах. По умолчанию 10 минут.
   /// </summary>
   public int ExpiresInSeconds { get; set; } = 600;
}
