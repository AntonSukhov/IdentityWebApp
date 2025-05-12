namespace IdentityWebApp.Tests.TestData.TokenAuthController;

/// <summary>
/// Данные для тестирования аутентификации существующего пользователя.
/// Проверяет, что при повторном запросе токена возвращается тот же токен.
/// </summary>
public class ExistedUserReturnSameTokenOnRepeatedRequestTestData: TheoryData<string, string>
{
    public ExistedUserReturnSameTokenOnRepeatedRequestTestData()
    {
        Add("realcomrade2011@gmail.com", "ZZTop29121986_");
    }
}
