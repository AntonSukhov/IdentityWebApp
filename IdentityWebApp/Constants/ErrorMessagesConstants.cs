namespace IdentityWebApp.Constants;

// <summary>
/// Константы сообщений об ошибках.
/// </summary>
internal static class ErrorMessagesConstants
{
    public const string KeySizeOutOfRange = "Размер ключа должен быть положительным.";
    public const string ConnectionStringNotFound = "Строка подключения '{0}' не найдена.";
    public const string SectionNotFound = "Секция '{0}' не найдена.";
    public const string UserModelCannotBeNull = "Модель аутентификации не может быть null";
    public const string LoginCannotBeEmpty = "Логин не может быть пустым или состоять только из пробелов";
    public const string LoginLengthExceededWithLimit = "Длина логина не должна превышать {0} символов.";
    public const string PasswordCannotBeEmpty = "Пароль не может быть пустым или состоять только из пробелов";
}
