using System.Net;
using FluentAssertions;
using IdentityWebApp.Areas.Identity.Models;
using IdentityWebApp.Common.Extensions;
using IdentityWebApp.Common.Services;
using IdentityWebApp.Controllers;
using IdentityWebApp.Tests.TestData.TokenAuthController;

namespace IdentityWebApp.Tests.TokenAuthControllerTests;

/// <summary>
/// Тесты для метода <see cref="TokenAuthController.LoginAsync"/>.
/// </summary>
public class LoginAsyncTests
{
    #region Поля

    private readonly HttpClient _httpClient = HttpClientService.CreateHttpClient();
    private readonly string _methodUrl = "http://localhost:5062/api/token-auth/login";

    #endregion

    #region Методы

    /// <summary>
    /// Тестирует метод аутентификации пользователя для существующего пользователя.
    /// Проверяет, что при повторном запросе токена возвращается тот же токен.
    /// </summary>
    /// <param name="email">Электронная почта пользователя.</param>
    /// <param name="password">Пароль пользователя для аутентификации.</param>
    /// <param name="rememberMe">Флаг, указывающий, следует ли запомнить информацию о пользователе
    /// для будущих сессий.</param>
    /// <returns>Асинхронная задача, представляющая результат выполнения теста.</returns>
    [Theory]
    [ClassData(typeof(ExistedUserTestData))]
    public async Task ForExistedUser(string email, string password, bool rememberMe)
    {
        var userLogin = new UserLoginModel { Email = email, Password = password, RememberMe = rememberMe };
        var expected = await _httpClient.PostAsync<UserLoginModel, TokenModel>(_methodUrl, userLogin);

        expected.Should()
                .NotBeNull()
                .And.Match<TokenModel>(p => !string.IsNullOrEmpty(p.Value) &&
                                             p.Expires > DateTime.UtcNow);
    }

    /// <summary>
    /// Тестирует метод аутентификации пользователя для существующего пользователя.
    /// Проверяет, что при повторном запросе токена возвращается тот же токен.
    /// </summary>
    /// <param name="email">Электронная почта пользователя.</param>
    /// <param name="password">Пароль пользователя для аутентификации.</param>
    /// <param name="rememberMe">Флаг, указывающий, следует ли запомнить информацию о пользователе
    /// для будущих сессий.</param>
    /// <returns>Асинхронная задача, представляющая результат выполнения теста.</returns>
    [Theory]
    [ClassData(typeof(ExistedUserReturnSameTokenOnRepeatedRequestTestData))]
    public async Task ForExistedUserReturnSameTokenOnRepeatedRequest(string email, string password, bool rememberMe)
    {
        var userLogin = new UserLoginModel { Email = email, Password = password, RememberMe = rememberMe };
        var expectedFirst = await _httpClient.PostAsync<UserLoginModel, TokenModel>(_methodUrl, userLogin);
        var expectedSecond = await _httpClient.PostAsync<UserLoginModel, TokenModel>(_methodUrl, userLogin);

        // Проверка, что первый токен не равен null
        expectedFirst.Should().NotBeNull();

        // Проверка, что второй токен не равен null
        expectedSecond.Should().NotBeNull();

        // Проверка, что значения токенов совпадают
        expectedFirst.Value.Should().Be(expectedSecond.Value);

        // Проверка, что время истечения токенов также совпадает
        expectedFirst.Expires.Should().Be(expectedSecond.Expires);
        
        // Проверка, что время истечения первого токена больше текущего времени
        expectedFirst.Expires.Should().BeAfter(DateTime.UtcNow);
                              
    }

    /// <summary>
    /// Тест проверки метода аутентификации пользователя для не существующего пользователя.
    /// </summary>
    /// <param name="email">Электронная почта пользователя.</param>
    /// <param name="password">Пароль пользователя для аутентификации.</param>
    /// <returns>Асинхронная задача, представляющая результат выполнения теста.</returns>
    [Theory]
    [ClassData(typeof(NonExistedUserTestData))]
    public async Task ForNotExistedUser(string email, string password)
    {
        var userLogin = new UserLoginModel { Email = email, Password = password };

        var expected = await Assert.ThrowsAsync<HttpRequestException>
        (
            async () => await _httpClient.PostAsync<UserLoginModel, TokenModel>(_methodUrl, userLogin)
        );

        expected.Should().NotBeNull()
                         .And.Match<HttpRequestException>(p => p.StatusCode == HttpStatusCode.Unauthorized);
                        
    }

    /// <summary>
    /// Тест проверки метода аутентификации пользователя для некорректных учетных данных пользователя.
    /// </summary>
    /// <param name="email">Электронная почта пользователя.</param>
    /// <param name="password">Пароль пользователя.</param>
    /// <returns>Асинхронная задача, представляющая результат выполнения теста.</returns>
    [Theory]
    [ClassData(typeof(IncorrectDataTestData))]
    public async Task ForIncorrectData(string email, string password)
    {
        var userLogin = new UserLoginModel { Email = email, Password = password };

        var expected = await Assert.ThrowsAsync<HttpRequestException>
        (
            async () => await _httpClient.PostAsync<UserLoginModel, TokenModel>(_methodUrl, userLogin)
        );

        expected.Should().NotBeNull()
                         .And.Match<HttpRequestException>(p => p.StatusCode == HttpStatusCode.BadRequest);
    }

    #endregion
}
