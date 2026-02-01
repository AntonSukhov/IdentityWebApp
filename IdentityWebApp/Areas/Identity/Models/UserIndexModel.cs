using System.ComponentModel.DataAnnotations;

namespace IdentityWebApp.Areas.Identity.Models;

/// <summary>
/// Модель профиля пользователя системы.
/// </summary>
public class UserIndexModel
{
    /// <summary>
    /// Получает или задаёт имя пользователя системы.
    /// </summary>
    [Display(Name = "Имя")]
    public required string Username { get; set; }

    /// <summary>
    /// Получает или задаёт номер телефона пользователя.
    /// </summary>
    [Phone]
    [Display(Name = "Номер телефона")]
    public string? PhoneNumber { get; set; }

}
