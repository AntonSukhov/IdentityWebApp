using System.Text.Json;
using IdentityWebApp.Api.Models;
using Infrastructure.Networks.Extensions;
using Infrastructure.Networks.Services;

namespace IdentityWebApp.Api.Services;

/// <summary>
/// Сервис аутентификации пользователей через API.
/// </summary>
public class AuthenticationService
{
    #region Поля

    private readonly string _tokenAuthPath; 
    private readonly HttpClient _httpClient;

    #endregion

    #region Конструкторы

    /// <summary>
    /// Конструктор сервиса аутентификации.
    /// </summary>
    public AuthenticationService()
    {
        _tokenAuthPath = "/api/token-auth";
        _httpClient = HttpClientService.CreateDefaultHttpClient(); 
    }

    #endregion

    #region Методы

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

        var userModel = new UserModel { Login = userName, Password = password };

        var baseAddress = $"http://{serverName}";

        if (port.HasValue)
        {
            baseAddress += $":{port.Value}";
        }

        baseAddress += _tokenAuthPath;

        _httpClient.BaseAddress = new Uri(baseAddress);

        try
        {
            var response = await _httpClient.PostAsync<UserModel, TokenModel>(baseAddress, userModel, JsonSerializerOptions.Default)
                ?? throw new HttpRequestException("Не удалось получить ответ от сервера.");

            return response; 
        }
        catch (HttpRequestException ex)
        {
            throw new HttpRequestException("Ошибка при выполнении запроса аутентификации.", ex);
        }
    }

    #endregion
}
