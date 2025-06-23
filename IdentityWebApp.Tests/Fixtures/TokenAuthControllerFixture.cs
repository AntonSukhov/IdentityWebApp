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
    /// Получает контроллер аутентификации пользователя системы.
    /// </summary>
    public TokenAuthController TokenAuthController { get; private set; }

    /// <summary>
    /// Получает сервис работы с кэшем.
    /// </summary>
    public ICacheService<string, string> CacheService { get; }

    public Mock<IConfiguration> ConfigurationMock { get; }

    public Mock<UserManager<ApplicationUser>> UserManagerMock { get; }

    public Mock<IOptions<JwtSettings>> JwtSettingsMock { get; }

    #endregion

    #region Конструкторы

    public TokenAuthControllerFixture()
    {
        ConfigurationMock = new Mock<IConfiguration>();
        ConfigurationMock.Setup(c => c[ConstantsService.ApiKeySectionName])
                         .Returns(CryptographyService.GenerateSecureKey());

        var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
        UserManagerMock = new Mock<UserManager<ApplicationUser>>(userStoreMock.Object, null, null, null, null, null, null,
             null, null);

        JwtSettingsMock = new Mock<IOptions<JwtSettings>>();
        JwtSettingsMock.Setup(p => p.Value)
                       .Returns(new JwtSettings { ExpiresInSeconds = 601 });

        var memoryCache = new MemoryCache(new MemoryCacheOptions());

        CacheService = new MemoryCacheService<string, string>(memoryCache);

        TokenAuthController = RefreshTokenAuthController();
    }


    #endregion

    #region Методы

    /// <summary>
    /// Актуализирует экземпляр контроллера аутентификации токенов.
    /// </summary>
    public TokenAuthController RefreshTokenAuthController()
    {
        TokenAuthController = new TokenAuthController(
            configuration: ConfigurationMock.Object,
            userManager: UserManagerMock.Object,
            jwtSettings: JwtSettingsMock.Object,
            cacheService: CacheService);

        return TokenAuthController;
    }

    #endregion
}
