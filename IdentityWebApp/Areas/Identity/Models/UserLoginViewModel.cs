using System.ComponentModel.DataAnnotations;

namespace IdentityWebApp.Areas.Identity.Models;

/// <summary>
/// Модель отображения информации для входа пользователя системы.
/// </summary>
public class UserLoginViewModel
{
    /// <summary>
    /// Получает или задаёт адрес электронной почты пользователя системы.
    /// </summary>
    /// <remarks>
    /// Это значение используется в качестве логина пользователя системы его аутентификации.
    /// </remarks>
    [Required]
    [EmailAddress]
    [Display(Name = "Адрес электронной почты")]
    public required string Email { get; set; }

    /// <summary>
    /// Получает или задаёт пароль пользователя системы.
    /// </summary>
    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    public required string Password { get; set; }

    /// <summary>
    /// Получает или задаёт запоминать информацию о пользователи системы.
    /// </summary>
    [Display(Name = "Запомнить меня?")]
    public bool RememberMe { get; set; }
}
