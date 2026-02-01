using System.Text.Json;
using IdentityWebApp.Api.Models;
using Infrastructure.Networks.Extensions;
using Infrastructure.Networks.Services;
using Infrastructure.Security.Services;
using Infrastructure.Shared.Helpers;

namespace IdentityWebApp.Api.Services;

/// <summary>
/// Сервис аутентификации пользователей через API.
/// </summary>
public class AuthenticationService
{

    private readonly HttpClient _httpClient;

    /// <summary>
    /// Инициализирует экземпляр <see cref="AuthenticationService"/>.
    /// </summary>
    public AuthenticationService() : this(HttpClientService.CreateDefaultHttpClient()) {}

    /// <summary>
    /// Инициализирует экземпляр <see cref="AuthenticationService"/>.
    /// </summary>
    /// <param name="httpClient">Клиент для работы с API.</param>
    public AuthenticationService(HttpClient httpClient)
    {
        ArgumentNullException.ThrowIfNull(httpClient);

        _httpClient = httpClient; 
    }

    /// <summary>
    /// Выполняет аутентификацию пользователя.
    /// </summary>
    /// <param name="serverName">Имя сервера.</param>
    /// <param name="port">Порт сервера (может быть null).</param>
    /// <param name="userName">Имя пользователя.</param>
    /// <param name="password">Пароль пользователя.</param>
    /// <returns>Модель токена.</returns>
    /// <exception cref="ArgumentException">Выбрасывается, если входные параметры некорректны.</exception>
    /// <exception cref="HttpRequestException">Выбрасывается, если произошла ошибка при выполнении запроса.</exception>
    public async Task<TokenModel> LoginAsync(string serverName, int? port, string userName, string password)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(serverName);
        ArgumentException.ThrowIfNullOrWhiteSpace(userName);
        ArgumentException.ThrowIfNullOrWhiteSpace(password);

        var userSecretsId = Helpers.ConfigurationHelper.GetUserSecretsId();
        var secret = GetSecret(userSecretsId);

        var passwordEncrypt = CryptographyService.Encrypt(password, secret);
        var userModel = new UserModel { Login = userName, Password = passwordEncrypt };

        var baseAddress = GetBaseUri(serverName, port);
        _httpClient.BaseAddress = new Uri(baseAddress);

        try
        {
            var response = await _httpClient.PostAsync<UserModel, TokenModel>(ConstantsService.LoginUrl, userModel,
                JsonSerializerOptions.Default)
                ?? throw new HttpRequestException("Не удалось получить ответ от сервера.");

            return response;
        }
        catch (HttpRequestException ex)
        {
            throw new HttpRequestException("Ошибка при выполнении запроса аутентификации.", ex);
        }
    }

    /// <summary>
    /// Получает секретный ключ для аутентификации.
    /// </summary>
    /// <param name="userSecretsId">Идентификатор пользовательских секретов.</param>
    /// <returns>Секретный ключ.</returns>
    /// <exception cref="HttpRequestException">Выбрасывается, если не удалось получить секрет.</exception>
    private static string GetSecret(string? userSecretsId)
    {
        if (string.IsNullOrWhiteSpace(userSecretsId))
            throw new HttpRequestException("Не удалось получить идентификатор пользовательских секретов.");

        var secret = ConfigurationHelper.GetSecret("SK:ServiceApiKey", userSecretsId);

        if (string.IsNullOrWhiteSpace(secret))
            throw new HttpRequestException("Не удалось получить пользовательский секрет.");

        return secret;
    }

    /// <summary>
    /// Получает базовый адрес для API.
    /// </summary>
    /// <param name="serverName">Имя сервера.</param>
    /// <param name="port">Порт сервера (может быть null).</param>
    /// <returns>Строка с базовым адресом.</returns>
    private string GetBaseUri(string serverName, int? port)
    {
        var uriBuilder = new UriBuilder
        {
            Scheme = Uri.UriSchemeHttp,
            Host = serverName,
            Port = port ?? 80
        };

        return uriBuilder.ToString();
    }
}
