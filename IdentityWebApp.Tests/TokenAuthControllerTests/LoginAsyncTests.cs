using System.Net;
using FluentAssertions;
using IdentityWebApp.Areas.Identity.Models;
using IdentityWebApp.Common.Extensions;
using IdentityWebApp.Common.Services;
using IdentityWebApp.Controllers;

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
    /// Тест проверки метода аутентификации пользователя для существующего пользователя.
    /// </summary>
    /// <param name="userLogin">Модель, содержащая учетные данные пользователя, включая имя пользователя и пароль.</param>
    /// <returns></returns>
    [Theory]
    [MemberData(nameof(GetForExistedUserTestData))]
    public async Task ForExistedUser(UserLoginModel userLogin)
    {
        var expected = await _httpClient.PostAsync<UserLoginModel, TokenModel>(_methodUrl, userLogin);

        expected.Should()
                .NotBeNull()
                .And.Match<TokenModel>(p => !string.IsNullOrEmpty(p.Value) &&
                                             p.Expires > DateTime.UtcNow);
    }

    /// <summary>
    /// Тест проверки метода аутентификации пользователя для не существующего пользователя.
    /// </summary>
    /// <param name="userLogin">Модель, содержащая учетные данные пользователя, включая имя пользователя и пароль.</param>
    /// <returns></returns>
    [Theory]
    [MemberData(nameof(GetForNotExistedUserTestData))]
    public async Task ForNotExistedUser(UserLoginModel userLogin)
    {
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
    /// <param name="userLogin">Модель, содержащая учетные данные пользователя, включая имя пользователя и пароль.</param>
    /// <returns></returns>
    [Theory]
    [MemberData(nameof(GetForIncorrectDataTestData))]
    public async Task ForIncorrectData(UserLoginModel userLogin)
    {
        var expected = await Assert.ThrowsAsync<HttpRequestException>
        (
            async () => await _httpClient.PostAsync<UserLoginModel, TokenModel>(_methodUrl, userLogin)
        );

        expected.Should().NotBeNull()
                         .And.Match<HttpRequestException>(p => p.StatusCode == HttpStatusCode.BadRequest);
    }

    /// <summary>
    /// Данные для тестирования аутентификации существующего пользователя.
    /// </summary>
    public static IEnumerable<object[]> GetForExistedUserTestData()
    {
        yield return new object[] 
        { 
            new UserLoginModel
            { 
                Email = "realcomrade2011@gmail.com", 
                Password = "ZZTop29121986_"
            }
        };
        yield return new object[] 
        { 
            new UserLoginModel
            { 
                Email = "realcomrade2011@gmail.com", 
                Password = "ZZTop29121986_",
                RememberMe = true
            }
        };
    }

    /// <summary>
    /// Данные для тестирования аутентификации несуществующего пользователя.
    /// </summary>
    public static IEnumerable<object[]> GetForNotExistedUserTestData()
    {
        yield return new object[] 
        { 
            new UserLoginModel
            { 
                Email = $"user_{Guid.NewGuid().ToString()}@mail.ru", 
                Password = short.MaxValue.ToString()
            }
        };
        yield return new object[] 
        { 
            new UserLoginModel
            { 
                Email = "realcomrade2011@gmail.com", 
                Password = short.MaxValue.ToString()
            }
        };
        yield return new object[] 
        { 
            new UserLoginModel
            { 
                Email = $"user_{Guid.NewGuid().ToString()}@mail.ru", 
                Password = "ZZTop29121986_"
            }
        };
    }

    /// <summary>
    /// Данные для тестирования аутентификации пользователя для некорректных входных данных.
    /// </summary>
    public static IEnumerable<object[]> GetForIncorrectDataTestData()
    {
        yield return new object[] 
        { 
            new UserLoginModel
            { 
                Email = $"user_{Guid.NewGuid().ToString()}", 
                Password = short.MaxValue.ToString()
            }
        };
        yield return new object[] 
        { 
            new UserLoginModel
            { 
                Email = string.Empty, 
                Password = short.MaxValue.ToString()
            }
        };
        yield return new object[] 
        { 
            new UserLoginModel
            { 
                Email = " ", 
                Password = short.MaxValue.ToString()
            }
        };
        yield return new object[] 
        { 
            new UserLoginModel
            { 
                Email = $"user_{Guid.NewGuid().ToString()}@mail.ru", 
                Password = " "
            }
        };
    }

    #endregion
}
