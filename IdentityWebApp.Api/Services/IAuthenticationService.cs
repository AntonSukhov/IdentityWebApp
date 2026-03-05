using IdentityWebApp.Api.Models;

namespace IdentityWebApp.Api.Services;

/// <summary>
/// Сервис аутентификации пользователей через API.
/// </summary>
public interface IAuthenticationService
{
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
    public Task<TokenModel> LoginAsync(
        string serverName, 
        int? port, 
        string userName, 
        string password,
        bool useHttps = true, 
        CancellationToken cancellationToken = default);
}
