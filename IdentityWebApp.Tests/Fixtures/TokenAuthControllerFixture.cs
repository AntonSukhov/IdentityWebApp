using IdentityWebApp.Common.Services;
using IdentityWebApp.Controllers;
using IdentityWebApp.Data;
using IdentityWebApp.Other.Settings;
using IdentityWebApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;

namespace IdentityWebApp.Tests.Fixtures;

/// <summary>
/// Фикстура для тестирования методов контроллера аутентификации пользователя системы.
/// </summary>
public class TokenAuthControllerFixture
{
    #region Свойства

    /// <summary>
    /// Получает Http-клиент.
    /// </summary>
    public HttpClient HttpClient { get; }

    /// <summary>
    /// Получает URL метода аутентификации пользователя системы.
    /// </summary>
    public string LoginMethodUrl { get; }

    /// <summary>
    /// Получает контроллер аутентификации пользователя системы.
    /// </summary>
    public TokenAuthController TokenAuthController { get; }

    /// <summary>
    /// Получает сервис работы с кэшем.
    /// </summary>
    public ICacheService<string, string> CacheService { get; }

    public Mock<IConfiguration> ConfigurationMock { get; }

    public Mock<UserManager<ApplicationUser >> UserManagerMock { get; }

    public Mock<IOptions<JwtSettings>> JwtSettingsMock { get; }

    #endregion

    #region Конструкторы

    public TokenAuthControllerFixture()
    {
        HttpClient = HttpClientService.CreateHttpClient();
        LoginMethodUrl = "http://localhost:5062/api/token-auth/login"; //TODO: как-то избавиться от константы.

        ConfigurationMock = new Mock<IConfiguration>();
        ConfigurationMock.Setup(c => c[ConstantsService.ApiKeySectionName])
                         .Returns(CryptographyService.GenerateSecureKey());

        var userStoreMock = new Mock<IUserStore<ApplicationUser >>();
        UserManagerMock = new Mock<UserManager<ApplicationUser>>(userStoreMock.Object, null, null, null, null, null, null,
             null, null);                          

        JwtSettingsMock = new Mock<IOptions<JwtSettings>>();
        JwtSettingsMock.Setup(p => p.Value)
                       .Returns(new JwtSettings());

        var memoryCache = new MemoryCache(new MemoryCacheOptions());
        
        CacheService  = new MemoryCacheService<string, string>(memoryCache);

        TokenAuthController = new TokenAuthController(
            configuration: ConfigurationMock.Object, 
            userManager: UserManagerMock.Object, 
            jwtSettings: JwtSettingsMock.Object, 
            cacheService: CacheService);
    }

    #endregion
}
