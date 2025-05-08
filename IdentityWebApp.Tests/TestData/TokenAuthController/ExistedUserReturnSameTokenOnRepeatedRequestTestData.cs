namespace IdentityWebApp.Tests.TestData.TokenAuthController;

/// <summary>
/// Данные для тестирования аутентификации существующего пользователя.
/// Проверяет, что при повторном запросе токена возвращается тот же токен.
/// </summary>
public class ExistedUserReturnSameTokenOnRepeatedRequestTestData: TheoryData<string, string, bool>
{
    public ExistedUserReturnSameTokenOnRepeatedRequestTestData()
    {
        Add("realcomrade2011@gmail.com", "ZZTop29121986_", false);
        Add("realcomrade2011@gmail.com", "ZZTop29121986_", true);
    }
}
