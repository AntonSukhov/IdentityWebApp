using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IdentityWebApp.Areas.Identity.Models;
using IdentityWebApp.Data;
using IdentityWebApp.Other.Settings;
using IdentityWebApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace IdentityWebApp.Controllers;

/// <summary>
/// Контроллер для аутентификации пользователей с использованием токенов.
/// </summary>
[ApiController]
[Route("api/token-auth")]
public class TokenAuthController : ControllerBase
{
    #region Поля
    
    private readonly IConfiguration _configuration;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly JwtSettings _jwtSettings;
    private readonly ICacheService<string, string> _userTokenCacheService;

    #endregion

    #region Конструкторы

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    /// <param name="configuration">Объект конфигурации приложения.</param>
    /// <param name="userManager">Объект управления пользователями.</param>
    /// <param name="jwtSettings">Настройки JWT-токена.</param>
    /// <param name="cacheService">Сервис работы с кэшем.</param>
    public TokenAuthController(IConfiguration configuration, UserManager<ApplicationUser> userManager, 
        IOptions<JwtSettings> jwtSettings, ICacheService<string, string> cacheService)
    {
        _configuration = configuration;
        _userManager = userManager;
        _jwtSettings = jwtSettings.Value;
        _userTokenCacheService = cacheService;
    }

    #endregion

    #region Методы

    /// <summary>
    /// Аутентификация пользователя с использованием предоставленных учетных данных.
    /// </summary>
    /// <param name="model">Модель, содержащая учетные данные пользователя, включая имя пользователя и пароль.</param>
    /// <returns>Результат аутентификации пользователя.</returns>
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] UserLoginModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        var user = await _userManager.FindByNameAsync(model.Login);

        if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
        {
            return Unauthorized();
        }

        var cachedToken = GetCachedToken(user.Id);

        if (cachedToken != null)
        {
            return Ok(cachedToken);
        }
     
        var token = await GenerateTokenAsync(user);

        return Ok(token);
    }

    #region Закрытые методы

    /// <summary>
    /// Получение кэшированного токена для пользователя.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <returns>Кэшированный токен или null, если токен недоступен или истек.</returns>
    private TokenModel? GetCachedToken(string userId)
    {
        if (_userTokenCacheService.TryGetValue(userId, out var cachedToken))
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(cachedToken);

            if (jwtToken.ValidTo > DateTime.UtcNow)
            {
                return new TokenModel
                {
                    Value = cachedToken,
                    Expires = jwtToken.ValidTo
                };
            }
        }

        return null;
    }

    /// <summary>
    /// Генерация нового JWT-токена для указанного пользователя.
    /// </summary>
    /// <param name="user">Пользователь, для которого генерируется токен.</param>
    /// <returns>Сгенерированный токен.</returns>
    private async Task<TokenModel> GenerateTokenAsync(ApplicationUser  user)
    {
        var authClaims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.UserName ?? string.Empty),
            new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var roles = await _userManager.GetRolesAsync(user);

        authClaims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var apiKey = _configuration[ConstantsService.ApiKeySectionName] ?? string.Empty;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(apiKey));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(authClaims),
            Expires = DateTime.UtcNow.AddSeconds(_jwtSettings.ExpiresInSeconds),
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenAsString = tokenHandler.WriteToken(token);

        _userTokenCacheService.Set(user.Id, tokenAsString, token.ValidTo);

        return new TokenModel
        {
            Value = tokenAsString,
            Expires = token.ValidTo
        };
    }

    #endregion

    #endregion
}
