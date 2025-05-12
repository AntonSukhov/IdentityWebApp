namespace IdentityWebApp.Tests.TestData.TokenAuthController;

/// <summary>
/// Данные для тестирования аутентификации несуществующего пользователя.
/// </summary>
public class NonExistedUserTestData : TheoryData<string, string>
{
    public NonExistedUserTestData()
    {
        Add($"user_{Guid.NewGuid()}@mail.ru", short.MaxValue.ToString());
        Add("realcomrade2011@gmail.com", short.MaxValue.ToString());
        Add($"user_{Guid.NewGuid()}@mail.ru", "ZZTop29121986_");
        Add($"user_{Guid.NewGuid()}", short.MaxValue.ToString());
        Add(string.Empty, short.MaxValue.ToString());
        Add(" ", short.MaxValue.ToString());
        Add($"user_{Guid.NewGuid()}@mail.ru", " ");
    }
}
