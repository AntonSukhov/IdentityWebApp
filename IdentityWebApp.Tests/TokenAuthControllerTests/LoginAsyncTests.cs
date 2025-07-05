using FluentAssertions;
using IdentityWebApp.Areas.Identity.Models;
using IdentityWebApp.Controllers;
using IdentityWebApp.Data;
using IdentityWebApp.Other.Settings;
using IdentityWebApp.Services;
using IdentityWebApp.Tests.Fixtures;
using IdentityWebApp.Tests.TestData.TokenAuthController;
using Infrastructure.Security.Services;
using Microsoft.AspNetCore.Mvc;

namespace IdentityWebApp.Tests.TokenAuthControllerTests;

/// <summary>
/// Тесты для метода <see cref="TokenAuthController.LoginAsync"/>.
/// </summary>
public class LoginAsyncTests : IClassFixture<TokenAuthControllerFixture>
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
        var userLogin = new UserLoginModel { Login = login, Password = password };
        var user = new ApplicationUser { Id = Guid.NewGuid().ToString(), Email = userLogin.Login, UserName = userLogin.Login };
        var secretKeyValue = _fixture.ConfigurationMock.Object[ConstantsService.SKeySectionName] ?? string.Empty;
        var passwordDecrypt = CryptographyService.Decrypt(userLogin.Password, secretKeyValue);

        _fixture.UserManagerMock.Setup(p => p.FindByNameAsync(userLogin.Login))
                                .Returns(Task.FromResult<ApplicationUser?>(user));

        _fixture.UserManagerMock.Setup(p => p.CheckPasswordAsync(user, passwordDecrypt))
                                .Returns(Task.FromResult(true));

        _fixture.UserManagerMock.Setup(p => p.GetRolesAsync(user))
                                .Returns(Task.FromResult<IList<string>>([]));

        _fixture.RefreshTokenAuthController();


        var actual = await _fixture.TokenAuthController.LoginAsync(userLogin);

        actual.Should().BeOfType<OkObjectResult>()
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
        var user = new ApplicationUser { Id = Guid.NewGuid().ToString(), Email = userLogin.Login, UserName = userLogin.Login };
        var secretKeyValue = _fixture.ConfigurationMock.Object[ConstantsService.SKeySectionName] ?? string.Empty;
        var passwordDecrypt = CryptographyService.Decrypt(userLogin.Password, secretKeyValue);

        _fixture.UserManagerMock.Setup(p => p.FindByNameAsync(userLogin.Login))
                                .Returns(Task.FromResult<ApplicationUser?>(user));

        _fixture.UserManagerMock.Setup(p => p.CheckPasswordAsync(user, passwordDecrypt))
                                .Returns(Task.FromResult(true));

        _fixture.UserManagerMock.Setup(p => p.GetRolesAsync(user))
                                .Returns(Task.FromResult<IList<string>>([]));

        _fixture.RefreshTokenAuthController();

        var actualFirst = (await _fixture.TokenAuthController.LoginAsync(userLogin)) as OkObjectResult;
        var actualSecond = (await _fixture.TokenAuthController.LoginAsync(userLogin)) as OkObjectResult;

        // Проверка, что первый токен не равен null
        actualFirst.Should().NotBeNull();
        var tokenModelFirst = actualFirst.Value as TokenModel;
        tokenModelFirst.Should().NotBeNull();

        // Проверка, что второй токен не равен null
        actualSecond.Should().NotBeNull();
        var tokenModelSecond = actualSecond.Value as TokenModel;
        tokenModelSecond.Should().NotBeNull();

        // Проверка, что значения токенов совпадают
        tokenModelFirst.Value.Should().Be(tokenModelSecond.Value);

        // Проверка, что время истечения токенов также совпадает
        tokenModelFirst.Expires.Should().Be(tokenModelSecond.Expires);

        // Проверка, что время истечения первого токена больше текущего времени
        tokenModelFirst.Expires.Should().BeAfter(DateTime.UtcNow);

    }

    /// <summary>
    /// Тестирует метод аутентификации пользователя для существующего пользователя и малого времени жизни токена.
    /// </summary>
    /// <param name="login">Логин пользователя.</param>
    /// <param name="password">Пароль пользователя для аутентификации.</param>
    /// <param name="tokenLifeTime">Время жизни токена в секундах.</param>
    /// <returns>Асинхронная задача, представляющая результат выполнения теста.</returns>
    [Theory]
    [ClassData(typeof(ExistedUserAndShortTokenLifetimeTestData))]
    public async Task ForExistedUserAndShortTokenLifetime(string login, string password, int tokenLifeTime)
    {
        var userLogin = new UserLoginModel { Login = login, Password = password };
        var user = new ApplicationUser { Id = Guid.NewGuid().ToString(), Email = userLogin.Login, UserName = userLogin.Login };
        var secretKeyValue = _fixture.ConfigurationMock.Object[ConstantsService.SKeySectionName] ?? string.Empty;
        var passwordDecrypt = CryptographyService.Decrypt(userLogin.Password, secretKeyValue);
        var millisecondsDelay = tokenLifeTime * 1000 + 1;

        _fixture.JwtSettingsMock.Setup(p => p.Value)
                                .Returns(new JwtSettings { ExpiresInSeconds = tokenLifeTime });

        _fixture.UserManagerMock.Setup(p => p.FindByNameAsync(userLogin.Login))
                                .Returns(Task.FromResult<ApplicationUser?>(user));

        _fixture.UserManagerMock.Setup(p => p.CheckPasswordAsync(user, passwordDecrypt))
                                .Returns(Task.FromResult(true));

        _fixture.UserManagerMock.Setup(p => p.GetRolesAsync(user))
                                .Returns(Task.FromResult<IList<string>>([]));

        _fixture.RefreshTokenAuthController();

        var actualFirst = (await _fixture.TokenAuthController.LoginAsync(userLogin)) as OkObjectResult;

        await Task.Delay(millisecondsDelay);

        var actualSecond = (await _fixture.TokenAuthController.LoginAsync(userLogin)) as OkObjectResult;

        // Проверка, что первый токен не равен null
        actualFirst.Should().NotBeNull();
        var tokenModelFirst = actualFirst.Value as TokenModel;
        tokenModelFirst.Should().NotBeNull();

        // Проверка, что второй токен не равен null
        actualSecond.Should().NotBeNull();
        var tokenModelSecond = actualSecond.Value as TokenModel;
        tokenModelSecond.Should().NotBeNull();

        // Проверка, что значения токенов не совпадают
        tokenModelFirst.Value.Should().NotBe(tokenModelSecond.Value);

        // Проверка, что время истечения токенов также не совпадает
        tokenModelFirst.Expires.Should().NotBe(tokenModelSecond.Expires);

        // Проверка, что время истечения первого токена меньше текущего времени
        tokenModelFirst.Expires.Should().BeBefore(DateTime.UtcNow);
    }

    /// <summary>
    /// Тест проверки метода аутентификации пользователя для несуществующего пользователя.
    /// </summary>
    /// <param name="login">Логин пользователя.</param>
    /// <param name="password">Пароль пользователя для аутентификации.</param>
    /// <returns>Асинхронная задача, представляющая результат выполнения теста.</returns>
    [Theory]
    [ClassData(typeof(NonExistedUserTestData))]
    public async Task ForNotExistedUser(string login, string password)
    {
        var userLogin = new UserLoginModel { Login = login, Password = password };
        var user = new ApplicationUser { Id = Guid.NewGuid().ToString(), Email = userLogin.Login, UserName = userLogin.Login };
        var secretKeyValue = _fixture.ConfigurationMock.Object[ConstantsService.SKeySectionName] ?? string.Empty;
        var passwordDecrypt = CryptographyService.Decrypt(userLogin.Password, secretKeyValue);

        _fixture.UserManagerMock.Setup(p => p.FindByNameAsync(userLogin.Login))
                                .Returns(Task.FromResult<ApplicationUser?>(null));

        _fixture.UserManagerMock.Setup(p => p.CheckPasswordAsync(user, passwordDecrypt))
                                .Returns(Task.FromResult(false));

        _fixture.UserManagerMock.Setup(p => p.GetRolesAsync(user))
                                .Returns(Task.FromResult<IList<string>>([]));

        _fixture.RefreshTokenAuthController();


        var actual = await _fixture.TokenAuthController.LoginAsync(userLogin);

        actual.Should().NotBeNull()
                       .And.BeOfType<UnauthorizedResult>();
    }
    
    /// <summary>
    /// Тест проверки метода аутентификации пользователя для пользователя с пустым или содержащим только пробелы паролем.
    /// </summary>
    /// <param name="login">Логин пользователя.</param>
    /// <param name="password">Пароль пользователя для аутентификации.</param>
    /// <returns>Асинхронная задача, представляющая результат выполнения теста.</returns>
    [Theory]
    [ClassData(typeof(UserWithEmptyOrWhitespacePasswordTestData))]
    public async Task ForUserWithEmptyOrWhitespacePassword(string login, string password)
    {
        var userLogin = new UserLoginModel { Login = login, Password = password};
        var user = new ApplicationUser { Id = Guid.NewGuid().ToString(), Email = userLogin.Login, UserName = userLogin.Login };
   
        _fixture.UserManagerMock.Setup(p => p.FindByNameAsync(userLogin.Login))
                                .Returns(Task.FromResult<ApplicationUser?>(user));

        _fixture.UserManagerMock.Setup(p => p.CheckPasswordAsync(user, userLogin.Password))
                                .Returns(Task.FromResult(false));

        _fixture.UserManagerMock.Setup(p => p.GetRolesAsync(user))
                                .Returns(Task.FromResult<IList<string>>([]));

        _fixture.RefreshTokenAuthController();


        var func = async () => await _fixture.TokenAuthController.LoginAsync(userLogin);

        await func.Should().ThrowAsync<ArgumentException>();                  
    }

    #endregion
}
