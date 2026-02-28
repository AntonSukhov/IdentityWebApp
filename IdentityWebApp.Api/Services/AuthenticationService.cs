using System.Net.Http.Json;
using System.Text.Json;
using IdentityWebApp.Api.Models;
using Infrastructure.Networks.Services;

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
        _httpClient.Timeout = TimeSpan.FromSeconds(
            ConstantsService.DefaultHttpClientTimeoutSeconds);
    }

    /// <summary>
    /// Выполняет аутентификацию пользователя.
    /// </summary>
    /// <param name="serverName">Имя сервера.</param>
    /// <param name="port">Порт сервера (может быть <c>null</c>).</param>
    /// <param name="userName">Имя пользователя.</param>
    /// <param name="password">Пароль пользователя.</param>
    /// <param name="useHttps">Использовать протокол Https. По умолчанию <c>true</c>.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Модель токена.</returns>
    public async Task<TokenModel> LoginAsync(
        string serverName, 
        int? port, 
        string userName, 
        string password,
        bool useHttps = true, 
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(serverName);
        ArgumentException.ThrowIfNullOrWhiteSpace(userName);
        ArgumentException.ThrowIfNullOrWhiteSpace(password);

        var userModel = new UserModel { Login = userName, Password = password };

        var baseAddress = GetBaseUri(serverName, port);
        _httpClient.BaseAddress = new Uri(baseAddress);

        try
        {
            var response = await _httpClient.PostAsJsonAsync(
                ConstantsService.LoginUri,
                userModel,
                JsonSerializerOptions.Default,
                cancellationToken);
                
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    throw new InvalidOperationException(ConstantsService.ErrorInvalidCredentials);
                
                var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);

                throw new HttpRequestException(
                    string.Format(ConstantsService.ErrorServerError, response.StatusCode, errorContent));
            
            }

            var token = await response.Content.ReadFromJsonAsync<TokenModel>(cancellationToken) 
                ?? throw new JsonException(ConstantsService.ErrorTokenDeserializationFailed);

            return token;
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw new OperationCanceledException(
                ConstantsService.ErrorAuthenticationCancelled, cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            throw new IOException(ConstantsService.ErrorConnectionFailed, ex);
        }
        catch (UriFormatException ex)
        {
            throw new InvalidOperationException(ConstantsService.ErrorInvalidServerAddress, ex);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                ConstantsService.ErrorUnexpectedAuthenticationError, ex);
        }
    }

    /// <summary>
    /// Получает базовый адрес для API.
    /// </summary>
    /// <param name="serverName">Имя сервера.</param>
    /// <param name="port">Порт сервера (может быть <c>null</c>).</param>
    /// <param name="useHttps">Использовать протокол Https. По умолчанию <c>true</c>.</param>
    /// <returns>Строка с базовым адресом.</returns>
    private static string GetBaseUri(string serverName, int? port, bool useHttps = true)
    {
        var scheme = useHttps
            ? ConstantsService.HttpsApiScheme 
            : ConstantsService.HttpApiScheme;
            
        var defaultPort = useHttps
            ? ConstantsService.DefaultHttpsPort
            : ConstantsService.DefaultHttpPort;

        var uriBuilder = new UriBuilder
        {
            Scheme = scheme ,
            Host = serverName,
            Port = port ?? defaultPort
        };

        return uriBuilder.ToString();
    }
}
