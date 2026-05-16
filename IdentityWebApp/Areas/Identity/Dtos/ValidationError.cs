namespace IdentityWebApp.Areas.Identity.Dtos;

/// <summary>
/// Ошибка валидации.
/// </summary>
/// <param name="ErrorKey">Ключ контекста ошибки.</param>
/// <param name="ErrorMessage">Текст сообщения об ошибке.</param>
public record ValidationError(string ErrorKey, string ErrorMessage);