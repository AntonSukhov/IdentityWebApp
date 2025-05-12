namespace IdentityWebApp.Tests.TestData.TokenAuthController;

/// <summary>
/// Данные для тестирования аутентификации существующего пользователя.
/// </summary>
public class ExistedUserTestData : TheoryData<string, string>
{
    public ExistedUserTestData()
    {
        Add("my_test_email@mail.ru", "my_test_password_1345");
    }
}
