using System.Net.Http.Json;
using System.Text.Json;
using IdentityWebApp.Api.Constants;
using IdentityWebApp.Api.Models;

namespace IdentityWebApp.Api.Services;

/// <summary>
/// Реализация сервиса аутентификации пользователей через API.
/// </summary>
public class AuthenticationService: IAuthenticationService
{
    private readonly IHttpClientFactory _httpClientFactory;

    /// <summary>
    /// Инициализирует экземпляр <see cref="AuthenticationService"/>.
    /// </summary>
    /// <param name="httpClientFactory">Фабрика создания http-клиента.</param>
    public AuthenticationService(IHttpClientFactory httpClientFactory)
    {
        ArgumentNullException.ThrowIfNull(httpClientFactory);

        _httpClientFactory =  httpClientFactory;
    }

    /// <inheritdoc/>
    public async Task<TokenModel> LoginAsync(
        string userName, 
        string password,
        CancellationToken cancellationToken = default)
    {
        var userModel = new UserModel { Login = userName, Password = password };

        // Получаем именованный клиент с настройками из DI
        var httpClient = _httpClientFactory.CreateClient(ApiConstants.HttpClientName);

        try
        {
            var response = await httpClient.PostAsJsonAsync(
                ApiConstants.LoginUri,
                userModel,
                JsonSerializerOptions.Default,
                cancellationToken);
                
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    throw new InvalidOperationException(ErrorMessagesConstants.InvalidCredentials);
                
                var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);

                throw new HttpRequestException(
                    string.Format(ErrorMessagesConstants.ServerError, response.StatusCode, errorContent));
            
            }

            var token = await response.Content.ReadFromJsonAsync<TokenModel>(cancellationToken) 
                ?? throw new JsonException(ErrorMessagesConstants.TokenDeserializationFailed);

            return token;
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw new OperationCanceledException(
                ErrorMessagesConstants.AuthenticationCancelled, cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            throw new IOException(ErrorMessagesConstants.ConnectionFailed, ex);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                ErrorMessagesConstants.UnexpectedAuthenticationError, ex);
        }
    }
}
