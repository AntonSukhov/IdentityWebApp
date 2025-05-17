namespace IdentityWebApp.Tests.TestData.TokenAuthController;

/// <summary>
/// Данные для тестирования аутентификации несуществующего пользователя.
/// </summary>
public class NonExistedUserTestData : TheoryData<string, string>
{
    public NonExistedUserTestData()
    {
        Add($"user_{Guid.NewGuid().ToString()}@mail.ru", $"{Guid.NewGuid().ToString()}");
        Add("realcomrade2011@gmail.com", $"{Guid.NewGuid().ToString()}");
        Add($"user_{Guid.NewGuid()}@mail.ru", "ZZTop29121986_");
        Add($"user_{Guid.NewGuid()}", $"{Guid.NewGuid().ToString()}");
        Add(string.Empty, $"{Guid.NewGuid().ToString()}");
        Add(" ", $"{Guid.NewGuid().ToString()}");
        Add($"user_{Guid.NewGuid()}@mail.ru", " ");
    }
}
