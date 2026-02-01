using IdentityWebApp.Areas.Identity.Models;
using IdentityWebApp.Data;
using IdentityWebApp.Tests.TestSupport.Constants;
using Infrastructure.Testing.TestCases;
using Infrastructure.Testing.XUnit;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace IdentityWebApp.Tests.Controllers.TokenAuthController.LoginAsync;

/// <summary>
/// Тесты для метода <see cref="IdentityWebApp.Controllers.TokenAuthController.LoginAsync"/>.
/// </summary>
public class LoginAsyncTests : BaseTest<TokenAuthControllerFixture>
{
    /// <summary>
    /// Инициализирует экземпляр <see cref="LoginAsyncTests"/>.
    /// </summary>
    /// <param name="fixture">Настройка контекста для тестирования 
    /// контроллера аутентификации пользователя системы.</param>
    public LoginAsyncTests(TokenAuthControllerFixture fixture) : base(fixture) { }
    
    /// <summary>
    /// Проверяет, что метод <see cref="LoginAsync"/> успешно выполняется для существующего пользователя.
    /// </summary>
    [Theory]
    [MemberData(nameof(LoginAsyncTestCases.ExistedUserTestCases),
                MemberType = typeof(LoginAsyncTestCases))]
    public async Task SucceedsForExistedUser(TestCaseInputWithStubs<UserLoginModel> testCase)
    {
        // Arrange:
        var stubOutput = testCase.StubOutputs[(UserManagerMethodNames.FindByNameAsync,
            StubSequenceConstants.First)];
        var stubOutputData = stubOutput.GetOutputData<ApplicationUser>();

        _fixture.UserManagerMock.Setup(p => p.FindByNameAsync(It.IsAny<string>()))
                                .ReturnsAsync(stubOutputData);

        _fixture.UserManagerMock.Setup(p => p.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                                .ReturnsAsync(true);

        _fixture.UserManagerMock.Setup(p => p.GetRolesAsync(It.IsAny<ApplicationUser>()))
                                .ReturnsAsync([]);

        // Act: 
        var result = await _fixture.TokenAuthController.LoginAsync(testCase.InputData);

        // Assert: 
        var objectResult = Assert.IsType<OkObjectResult>(result);
        var token = Assert.IsType<TokenModel>(objectResult.Value);

        Assert.NotNull(token);      
        Assert.True(!string.IsNullOrEmpty(token.Value) && token.Expires> DateTime.UtcNow);
    }

    /// <summary>
    /// Проверяет метод <see cref="LoginAsync"/> для существующего пользователя с временем жизни токена, 
    /// большем чем период времени между повторными запросами.
    /// </summary>
    [Theory]
    [MemberData(nameof(LoginAsyncTestCases.ExistedUserTokenLifetimeExceedsRequestIntervalTestCases),
                MemberType = typeof(LoginAsyncTestCases))]
    public async Task ExistedUserTokenLifetimeExceedsRequestInterval(
        TestCaseInputWithStubs<UserLoginModel> testCase)
    {
          // Arrange:
        var stubOutput = testCase.StubOutputs[(UserManagerMethodNames.FindByNameAsync,
            StubSequenceConstants.First)];
        var stubOutputData = stubOutput.GetOutputData<ApplicationUser>();

        _fixture.UserManagerMock.Setup(p => p.FindByNameAsync(It.IsAny<string>()))
                                .ReturnsAsync(stubOutputData);

        _fixture.UserManagerMock.Setup(p => p.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                                .ReturnsAsync(true);

        _fixture.UserManagerMock.Setup(p => p.GetRolesAsync(It.IsAny<ApplicationUser>()))
                                .ReturnsAsync([]);

        // Act:
        var firstResult = await _fixture.TokenAuthController.LoginAsync(testCase.InputData);
        var secondResult = await _fixture.TokenAuthController.LoginAsync(testCase.InputData);

        // Assert: 

        // Проверка, что первый токен не равен null
        Assert.NotNull(firstResult);
        var firstObjectResult = Assert.IsType<OkObjectResult>(firstResult);
        var firstToken = Assert.IsType<TokenModel>(firstObjectResult.Value);
        Assert.NotNull(firstToken);

        // Проверка, что второй токен не равен null
        Assert.NotNull(secondResult);
        var secondObjectResult = Assert.IsType<OkObjectResult>(secondResult);
        var secondToken = Assert.IsType<TokenModel>(secondObjectResult.Value);
        Assert.NotNull(secondToken);

        // Проверка, что значения токенов не совпадают
        Assert.Equal(firstToken.Value, secondToken.Value);

        // Проверка, что время истечения токенов также не совпадает
        Assert.Equal(firstToken.Expires, secondToken.Expires);

        // Проверка, что время истечения первого токена больше текущего времени
        Assert.True(firstToken.Expires > DateTime.UtcNow);
    }

    /// <summary>
    /// Проверяет метод <see cref="LoginAsync"/> для существующего пользователя с малым временем жизни токена.
    /// </summary>
    [Theory]
    [MemberData(nameof(LoginAsyncTestCases.ExistedUserWithShortTokenLifetimeTestCases),
                MemberType = typeof(LoginAsyncTestCases))]
    public async Task ExistedUserWithShortTokenLifetime(
        TestCaseInputWithStubs<UserContext> testCase)
    {
        // Arrange:
        var stubOutput = testCase.StubOutputs[(UserManagerMethodNames.FindByNameAsync,
            StubSequenceConstants.First)];
        var stubOutputData = stubOutput.GetOutputData<ApplicationUser>();
        var millisecondsDelay = testCase.InputData.TokenLifetimeSeconds * 1000 + 10;
        var jwtSettings = TokenAuthControllerFixture.CreateJwtSettings(testCase.InputData.TokenLifetimeSeconds);

        _fixture.JwtSettingsMock.Setup(p => p.Value)
                                .Returns(jwtSettings);

        _fixture.UserManagerMock.Setup(p => p.FindByNameAsync(It.IsAny<string>()))
                                .ReturnsAsync(stubOutputData);

        _fixture.UserManagerMock.Setup(p => p.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                                .ReturnsAsync(true);

        _fixture.UserManagerMock.Setup(p => p.GetRolesAsync(It.IsAny<ApplicationUser>()))
                                .ReturnsAsync([]);

        _fixture.RecreateTokenAuthController();

        // Act:
        var firstResult = await _fixture.TokenAuthController.LoginAsync(testCase.InputData.UserLogin);

        await Task.Delay(millisecondsDelay);

        var secondResult = await _fixture.TokenAuthController.LoginAsync(testCase.InputData.UserLogin);

        // Assert: 

        // Проверка, что первый токен не равен null
        Assert.NotNull(firstResult);
        var firstObjectResult = Assert.IsType<OkObjectResult>(firstResult);
        var firstToken = Assert.IsType<TokenModel>(firstObjectResult.Value);
        Assert.NotNull(firstToken);

        // Проверка, что второй токен не равен null
        Assert.NotNull(secondResult);
        var secondObjectResult = Assert.IsType<OkObjectResult>(secondResult);
        var secondToken = Assert.IsType<TokenModel>(secondObjectResult.Value);
        Assert.NotNull(secondToken);

        // Проверка, что значения токенов не совпадают
        Assert.NotEqual(firstToken.Value, secondToken.Value);

        // Проверка, что время истечения токенов также не совпадает
        Assert.NotEqual(firstToken.Expires, secondToken.Expires);

        // Проверка, что время истечения первого токена меньше текущего времени
        Assert.True(firstToken.Expires < DateTime.UtcNow);
    }

    /// <summary>
    /// Проверяет, что метод <see cref="LoginAsync"/> не выполняет аутентификацию для несуществующего пользователя.
    /// </summary>
    [Theory]
    [MemberData(nameof(LoginAsyncTestCases.UnknownUserTestCases),
                MemberType = typeof(LoginAsyncTestCases))]
    public async Task NoTokenForUnknownUser(TestCaseInput<UserLoginModel> testCase)
    {
        // Arrange:
        _fixture.UserManagerMock.Setup(p => p.FindByNameAsync(It.IsAny<string>()))
                                .ReturnsAsync((ApplicationUser?)null);

        _fixture.UserManagerMock.Setup(p => p.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                                .ReturnsAsync(false);

        _fixture.UserManagerMock.Setup(p => p.GetRolesAsync(It.IsAny<ApplicationUser>()))
                                .ReturnsAsync([]);

        // Act: 
        var result = await _fixture.TokenAuthController.LoginAsync(testCase.InputData);

        // Assert: 
        Assert.NotNull(result);  
        Assert.IsType<UnauthorizedResult>(result);   
    }
        
    /// <summary>
    /// Проверяет, что метод <see cref="LoginAsync"/> не выполняет аутентификацию 
    /// для пользователя c пустым или содержащим только пробелы паролем.
    /// </summary>
    [Theory]
    [MemberData(nameof(LoginAsyncTestCases.EmptyOrWhitespacePasswordTestCases),
                MemberType = typeof(LoginAsyncTestCases))]
    public async Task FailsForUserWithEmptyOrWhitespacePassword(
        TestCaseInputWithStubs<UserLoginModel> testCase)
    {
        // Arrange:
        var stubOutput = testCase.StubOutputs[(UserManagerMethodNames.FindByNameAsync,
            StubSequenceConstants.First)];
        var stubOutputData = stubOutput.GetOutputData<ApplicationUser>();

        _fixture.UserManagerMock.Setup(p => p.FindByNameAsync(It.IsAny<string>()))
                                .ReturnsAsync(stubOutputData);

        _fixture.UserManagerMock.Setup(p => p.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                                .ReturnsAsync(false);

        _fixture.UserManagerMock.Setup(p => p.GetRolesAsync(It.IsAny<ApplicationUser>()))
                                .Returns(Task.FromResult<IList<string>>([]));

        // Act & Assert: 
        var exception = await Assert.ThrowsAnyAsync<Exception>(
            async () => await _fixture.TokenAuthController.LoginAsync(testCase.InputData)
        );

        // Проверяем, что исключение относится к разрешённым типам
        Assert.True(
            exception is ArgumentException
        );
    }
}
