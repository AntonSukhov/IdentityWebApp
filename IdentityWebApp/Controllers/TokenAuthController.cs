using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IdentityWebApp.Areas.Identity.Models;
using IdentityWebApp.Data;
using IdentityWebApp.Other.Settings;
using IdentityWebApp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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

    #endregion

    #region Конструкторы

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    /// <param name="configuration">Объект конфигурации приложения.</param>
    /// <param name="userManager">Объект управления пользователями.</param>
    /// <param name="jwtSettings">Настройки JWT-токена.</param>
    public TokenAuthController(IConfiguration configuration, UserManager<ApplicationUser> userManager, 
    IOptions<JwtSettings> jwtSettings)
    {
        _configuration = configuration;
        _userManager = userManager;
        _jwtSettings = jwtSettings.Value;
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

        var user = await _userManager.FindByNameAsync(model.Email);

        if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
        {
            return Unauthorized();
        }

        var authClaims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.UserName ?? string.Empty),
            new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
         

        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, role));
        }

        var apiKey = _configuration[ConstantsService.ApiKeySectionName] ?? string.Empty;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(apiKey));

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(authClaims),
            Expires = DateTime.Now.AddMinutes(_jwtSettings.ExpiresInMinutes),
            SigningCredentials = new SigningCredentials(key , SecurityAlgorithms.HmacSha512Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return Ok(new TokenModel
        {
            Value = tokenHandler.WriteToken(token),
            Expires = token.ValidTo
        });
    }

    /// <summary>
    /// Предоставляет результат аутентификации пользователя.
    /// </summary>
    /// <returns>Результат аутентификации пользователя.</returns>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("data")]
    public IActionResult Data()
    {
        var data = new []
        {
            new {Id = 1, Name = "Name1"}, 
            new {Id = 2, Name = "Name2"},
            new {Id = 3, Name = "Name3"}
        };

        return Ok(data);
    }

    #endregion
}
