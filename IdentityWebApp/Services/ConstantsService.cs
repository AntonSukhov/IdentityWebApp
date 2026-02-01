namespace IdentityWebApp.Services;

/// <summary>
/// Сервис работы с константами программы.
/// </summary>
public static class ConstantsService
{
    public const string DefaultConnectionSectionName = "DefaultConnection";
    public const string SmtpSettingsSectionName = "SmtpSettings";
    public const string JwtSettingsSectionName = "JwtSettings";
    public const string DefaultCookieName = "IdentityWebAppCookie";
    public const string JwtKeySectionName = "JWT:ServiceApiKey";
    public const string SKeySectionName = "SK:ServiceApiKey";
    public const string KeySizeOutOfRangeErrorMessage = "Размер ключа должен быть положительным.";

    /// <summary>
    /// Генерирует сообщение об ошибке, указывающее, что указанная секция не найдена.
    /// </summary>
    /// <param name="sectionName">Имя секции, которая не была найдена.</param>
    /// <returns>Сообщение об ошибке, указывающее на отсутствие секции.</returns>
    public static string GenerateSectionNotFoundErrorMessage(string sectionName)
    {
        return $"Секция '{sectionName}' не найдена.";
    }
}
