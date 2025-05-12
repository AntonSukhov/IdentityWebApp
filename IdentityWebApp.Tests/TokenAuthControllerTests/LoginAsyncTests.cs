using System.Net;
using FluentAssertions;
using IdentityWebApp.Areas.Identity.Models;
using IdentityWebApp.Common.Extensions;
using IdentityWebApp.Controllers;
using IdentityWebApp.Data;
using IdentityWebApp.Tests.Fixtures;
using IdentityWebApp.Tests.TestData.TokenAuthController;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace IdentityWebApp.Tests.TokenAuthControllerTests;

/// <summary>
/// Тесты для метода <see cref="TokenAuthController.LoginAsync"/>.
/// </summary>
public class LoginAsyncTests: IClassFixture<TokenAuthControllerFixture>
{
    #region Поля
    
    private readonly TokenAuthControllerFixture _fixture;

    #endregion

    #region Конструкторы

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    /// <param name="fixture">Настройка контекста для тестирования контроллера аутентификации пользователя системы.</param>
    public LoginAsyncTests(TokenAuthControllerFixture fixture)
    {
        _fixture = fixture;
    }

    #endregion

    #region Методы

    /// <summary>
    /// Тестирует метод аутентификации пользователя для существующего пользователя.
    /// Проверяет, что при повторном запросе токена возвращается тот же токен.
    /// </summary>
    /// <param name="login">Логин пользователя.</param>
    /// <param name="password">Пароль пользователя для аутентификации.</param>
    /// для будущих сессий.</param>
    /// <returns>Асинхронная задача, представляющая результат выполнения теста.</returns>
    [Theory]
    [ClassData(typeof(ExistedUserTestData))]
    public async Task ForExistedUser(string login, string password)
    {
        var userLogin = new UserLoginModel { Login = login, Password = password};
        var user = new ApplicationUser { Id = Guid.NewGuid().ToString(), Email = userLogin.Login, UserName = userLogin.Login };

        _fixture.UserManagerMock.Setup(p => p.FindByNameAsync(userLogin.Login))
                                .Returns(Task.FromResult<ApplicationUser?>(user));

        _fixture.UserManagerMock.Setup(p => p.CheckPasswordAsync(user, userLogin.Password))
                                .Returns(Task.FromResult(true));

        _fixture.UserManagerMock.Setup(p => p.GetRolesAsync(user))
                                .Returns(Task.FromResult<IList<string>>([]));


        var expected = await _fixture.TokenAuthController.LoginAsync(userLogin);

        expected.Should().BeOfType<OkObjectResult>()
                         .Which.Value.Should().BeOfType<TokenModel>()
                                              .And.NotBeNull()
                                              .And.Match<TokenModel>(p => !string.IsNullOrEmpty(p.Value) && 
                                                                           p.Expires > DateTime.UtcNow);
    }

    /// <summary>
    /// Тестирует метод аутентификации пользователя для существующего пользователя.
    /// Проверяет, что при повторном запросе токена возвращается тот же токен.
    /// </summary>
    /// <param name="login">Логин пользователя.</param>
    /// <param name="password">Пароль пользователя для аутентификации.</param>
    /// <returns>Асинхронная задача, представляющая результат выполнения теста.</returns>
    [Theory]
    [ClassData(typeof(ExistedUserReturnSameTokenOnRepeatedRequestTestData))]
    public async Task ForExistedUserReturnSameTokenOnRepeatedRequest(string login, string password)
    {
        var userLogin = new UserLoginModel { Login = login, Password = password };
        var expectedFirst = await _fixture.HttpClient.PostAsync<UserLoginModel, TokenModel>(_fixture.LoginMethodUrl, userLogin);
        var expectedSecond = await _fixture.HttpClient.PostAsync<UserLoginModel, TokenModel>(_fixture.LoginMethodUrl, userLogin);

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
    /// <param name="login">Логин пользователя.</param>
    /// <param name="password">Пароль пользователя для аутентификации.</param>
    /// <returns>Асинхронная задача, представляющая результат выполнения теста.</returns>
    [Theory]
    [ClassData(typeof(NonExistedUserTestData))]
    public async Task ForNotExistedUser(string login, string password)
    {
        var userLogin = new UserLoginModel { Login = login, Password = password };

        var expected = await Assert.ThrowsAsync<HttpRequestException>
        (
            async () => await _fixture.HttpClient.PostAsync<UserLoginModel, TokenModel>(_fixture.LoginMethodUrl, userLogin)
        );

        expected.Should().NotBeNull()
                         .And.Match<HttpRequestException>(p => p.StatusCode == HttpStatusCode.Unauthorized);
                        
    }

    #endregion
}
