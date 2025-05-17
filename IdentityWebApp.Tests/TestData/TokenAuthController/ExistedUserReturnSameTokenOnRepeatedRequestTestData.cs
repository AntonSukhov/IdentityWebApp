namespace IdentityWebApp.Tests.TestData.TokenAuthController;

/// <summary>
/// Данные для тестирования аутентификации существующего пользователя.
/// Проверяет, что при повторном запросе токена возвращается тот же токен.
/// </summary>
public class ExistedUserReturnSameTokenOnRepeatedRequestTestData: TheoryData<string, string>
{
    public ExistedUserReturnSameTokenOnRepeatedRequestTestData()
    {
        Add("email@mail.ru", "password1234");
    }
}
