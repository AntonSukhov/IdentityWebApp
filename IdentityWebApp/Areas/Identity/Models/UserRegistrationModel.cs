using System.ComponentModel.DataAnnotations;

namespace IdentityWebApp.Areas.Identity.Models;

/// <summary>
/// Модель регистрации пользователя системы.
/// </summary>
public class UserRegistrationModel
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
    /// Получает или задаёт имя пользователя системы.
    /// </summary>
    [Required]
    [Display(Name = "Имя")]
    public required string Name { get; set; }

    /// <summary>
    /// Получает или задаёт пароль пользователя системы.
    /// </summary>
    [Required]
    [StringLength(100, ErrorMessage = "{0} должен иметь длину не менее {2} и не более {1} символов.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    public required string Password { get; set; }

    /// <summary>
    /// Получает или задаёт подтверждение пароля пользователя системы.
    /// </summary>
    [DataType(DataType.Password)]
    [Display(Name = "Подтверждение пароля")]
    [Compare("Password", ErrorMessage = "Пароль и подтверждение пароля не совпадают.")]
    public string ConfirmPassword { get; set; } = string.Empty;
}