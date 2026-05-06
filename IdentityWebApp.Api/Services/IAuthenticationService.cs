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
    /// <param name="userName">Имя пользователя.</param>
    /// <param name="password">Пароль пользователя.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Модель токена.</returns>
    public Task<TokenModel> LoginAsync(
        string userName, 
        string password,
        CancellationToken cancellationToken = default);
}
