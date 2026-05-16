using IdentityWebApp.Areas.Identity.Dtos;
using IdentityWebApp.Areas.Identity.Models;
using IdentityWebApp.Constants;

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
    public static IReadOnlyCollection<ValidationError> Validate(UserLoginModel model)
    {
        var errors = new List<ValidationError>();

        if (model == null)
        {
            errors.Add(new ValidationError(
                nameof(UserLoginModel), 
                ErrorMessagesConstants.UserModelCannotBeNull 
            ));

            return errors.AsReadOnly();
        }

        if (string.IsNullOrWhiteSpace(model.Login))
        {
            errors.Add(new ValidationError(
                nameof(UserLoginModel.Login), 
                ErrorMessagesConstants.LoginCannotBeEmpty
            ));
        }
        else if(model.Login.Length > AppConstants.UserLoginMaxLength)
        {
            errors.Add(new ValidationError(
                nameof(UserLoginModel.Login), 
                GetLoginLengthErrorMessage()
            ));
        }

        if (string.IsNullOrWhiteSpace(model.Password))
        {
            errors.Add(new ValidationError(
                nameof(UserLoginModel.Password),
                ErrorMessagesConstants.PasswordCannotBeEmpty
            ));
        }

        return errors.AsReadOnly();
    }

    private static string GetLoginLengthErrorMessage()
    {
        return string.Format(
            ErrorMessagesConstants.LoginLengthExceededWithLimit,
            AppConstants.UserLoginMaxLength);
    }
}
