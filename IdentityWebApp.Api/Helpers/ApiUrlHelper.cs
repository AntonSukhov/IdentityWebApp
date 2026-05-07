using IdentityWebApp.Api.Constants;
using IdentityWebApp.Api.Settings;

namespace IdentityWebApp.Api.Helpers;

// <summary>
/// Помощник для работы с URL API.
/// </summary>
public static class ApiUrlHelper
{
    /// <summary>
    /// Получает базовый адрес для API на основе настроек аутентификации.
    /// </summary>
    /// <param name="authenticationSettings">Настройки аутентификации.</param>
    /// <returns>Базовый адрес API в виде URI.</returns>
    /// <exception cref="ArgumentException">
    /// Выбрасывается, если настройки не настроены или содержат некорректные данные.
    /// </exception>
    public static Uri GetBaseAddress(AuthenticationSettings? authenticationSettings)
    {
        if (authenticationSettings == null)
            throw new ArgumentException(ErrorMessagesConstants.AuthenticationSettingsNotConfigured);

        if (string.IsNullOrWhiteSpace(authenticationSettings.ServerName))
            throw new ArgumentException(ErrorMessagesConstants.ServerNameCannotBeEmpty);

        var scheme = authenticationSettings.UseHttps
            ? ApiConstants.HttpsApiScheme
            : ApiConstants.HttpApiScheme;

        var port = authenticationSettings.Port ?? (
            authenticationSettings.UseHttps
                ? ApiConstants.DefaultHttpsPort
                : ApiConstants.DefaultHttpPort
        );

        try
        {
            var uriBuilder = new UriBuilder
            {
                Scheme = scheme,
                Host = authenticationSettings.ServerName,
                Port = port
            };
            return uriBuilder.Uri;
        }
        catch (UriFormatException ex)
        {
            throw new ArgumentException(
                string.Format(ErrorMessagesConstants.InvalidUriFormat,
                    authenticationSettings.ServerName, port, scheme), ex);
        }
    }
}