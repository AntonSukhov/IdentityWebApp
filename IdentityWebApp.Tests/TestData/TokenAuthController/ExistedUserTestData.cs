namespace IdentityWebApp.Tests.TestData.TokenAuthController;

/// <summary>
/// Данные для тестирования аутентификации существующего пользователя.
/// </summary>
public class ExistedUserTestData : TheoryData<string, string, bool>
{
    public ExistedUserTestData()
    {
        Add("realcomrade2011@gmail.com", "ZZTop29121986_", false);
        Add("realcomrade2011@gmail.com", "ZZTop29121986_", true);
    }
}
