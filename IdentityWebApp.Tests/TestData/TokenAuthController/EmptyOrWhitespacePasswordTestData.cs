namespace IdentityWebApp.Tests.TestData.TokenAuthController;

/// <summary>
/// Данные для тестирования аутентификации пользователя с пустым или содержащим только пробелы паролем.
/// </summary>
public class UserWithEmptyOrWhitespacePasswordTestData : TheoryData<string, string>
{
    public UserWithEmptyOrWhitespacePasswordTestData ()
    {
        Add($"my_mail@mail.ru", string.Empty);
        Add($"user_{Guid.NewGuid()}@mail.ru", "  ");
    }
}
