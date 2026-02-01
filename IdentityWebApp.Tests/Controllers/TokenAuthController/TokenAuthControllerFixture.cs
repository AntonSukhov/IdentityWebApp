using IdentityWebApp.Data;
using IdentityWebApp.Other.Settings;
using IdentityWebApp.Services;
using Infrastructure.Caching.Services;
using Infrastructure.Security.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;

namespace IdentityWebApp.Tests.Controllers.TokenAuthController;

/// <summary>
/// Фикстура для тестирования методов контроллера <see cref="IdentityWebApp.Controllers.TokenAuthController"/>.
/// </summary>
public class TokenAuthControllerFixture
{
    /// <summary>
    /// Получает контроллер аутентификации пользователя системы.
    /// </summary>
    public IdentityWebApp.Controllers.TokenAuthController TokenAuthController { get; private set; }

    /// <summary>
    /// Получает сервис работы с кэшем.
    /// </summary>
    public ICacheService<string, string> CacheService { get; }

    /// <summary>
    /// Получает мок настроек свойств приложения.
    /// </summary>
    public Mock<IConfiguration> ConfigurationMock { get; }

    /// <summary>
    /// Получает мок для управления пользователем.
    /// </summary>
    public Mock<UserManager<ApplicationUser>> UserManagerMock { get; }

    /// <summary>
    /// Получает мок для настройки Jwt-токена.
    /// </summary>
    public Mock<IOptions<JwtSettings>> JwtSettingsMock { get; }

    /// <summary>
    /// Инициализирует экземпляр <see cref="TokenAuthControllerFixture"/>.
    /// </summary>
    public TokenAuthControllerFixture()
    {
        ConfigurationMock = CreateConfigurationMock(); 
        UserManagerMock = CreateUserManagerMock();
        JwtSettingsMock = CreateJwtSettingsMock();
        CacheService = CreateCacheService();

        TokenAuthController = RecreateTokenAuthController();
    }

    /// <summary>
    /// Пересоздает экземпляр <see cref="IdentityWebApp.Controllers.TokenAuthController"/>.
    /// </summary>
    public IdentityWebApp.Controllers.TokenAuthController RecreateTokenAuthController()
    {
        TokenAuthController = new IdentityWebApp.Controllers.TokenAuthController(
            configuration: ConfigurationMock.Object,
            userManager: UserManagerMock.Object,
            jwtSettings: JwtSettingsMock.Object,
            cacheService: CacheService);

        return TokenAuthController;
    }

    /// <summary>
    /// Создает экземпляр <see cref="JwtSettings"/>.
    /// </summary>
    /// <param name="expiresInSeconds"></param>
    /// <returns>Экземпляр <see cref="JwtSettings"/>.</returns>
    public static JwtSettings CreateJwtSettings(int expiresInSeconds = 601) 
        => new() { ExpiresInSeconds = expiresInSeconds };

    private static Mock<IConfiguration> CreateConfigurationMock()
    {
       var secretKey = "ZWbw+SVetnbgtsBvc5p6POkDHSWsgO+bN0qtWjnXBbE=";

       var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(c => c[ConstantsService.JwtKeySectionName])
                         .Returns(CryptographyService.GenerateSecureKey());
        configurationMock.Setup(c => c[ConstantsService.SKeySectionName])
                         .Returns(secretKey);

        return configurationMock;
    }
    
    private static Mock<UserManager<ApplicationUser>> CreateUserManagerMock()
    {
        var userStoreMock = new Mock<IUserStore<ApplicationUser>>();

        #pragma warning disable CS8625 // Отключаем предупреждение о null для моков

        var userManagerMock = new Mock<UserManager<ApplicationUser>>(
            userStoreMock.Object,
            null, // IPasswordHasher<ApplicationUser>
            null, // ILookupNormalizer
            null, // IdentityErrorDescriber
            null, // IServiceProvider
            null, // ILogger<UserManager<ApplicationUser>>
            null, // IHttpContextAccessor
            null, // IUserClaimsPrincipalFactory<ApplicationUser>
            null  // IEnumerable<IUserValidator<ApplicationUser>>
        );
        
        #pragma warning restore CS8625

        return userManagerMock;
    }

    private static Mock<IOptions<JwtSettings>> CreateJwtSettingsMock(int expiresInSeconds = 601)
    {
        var jwtSettingsMock = new Mock<IOptions<JwtSettings>>();

        jwtSettingsMock.Setup(p => p.Value)
                       .Returns(CreateJwtSettings(expiresInSeconds));

        return jwtSettingsMock;
    }

    private static ICacheService<string, string> CreateCacheService()
    {
        var memoryCache = new MemoryCache(new MemoryCacheOptions());

        var cacheService = new MemoryCacheService<string, string>(memoryCache);

        return cacheService;
    }
}
