using IdentityWebApp.Areas.Identity.Models;

namespace IdentityWebApp.Areas.Identity.Validators;

/// <summary>
/// Валидатор модели входа пользователя.
/// </summary>
/// <remarks>
/// Обеспечивает базовую проверку корректности данных для аутентификации пользователя.
/// </remarks>
public class UserLoginModelValidator
{
    /// <summary>
    /// Выполняет валидацию модели входа пользователя.
    /// </summary>
    /// <param name="model">Модель для валидации.</param>
    /// <returns> Сообщения об ошибках валидации.</returns>
    public static IReadOnlyCollection<string> Validate(UserLoginModel model)
    {
        var errors = new List<string>();

        if (model == null)
        {
            errors.Add("Модель аутентификации не может быть null");
            return errors.AsReadOnly();
        }

        if (string.IsNullOrWhiteSpace(model.Login))
        {
            errors.Add("Логин не может быть пустым или состоять только из пробелов");
        }

        if (string.IsNullOrWhiteSpace(model.Password))
        {
            errors.Add("Пароль не может быть пустым или состоять только из пробелов");
        }

        return errors.AsReadOnly();
    }
}
