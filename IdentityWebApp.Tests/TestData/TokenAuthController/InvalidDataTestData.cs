namespace IdentityWebApp.Tests.TestData.TokenAuthController;

/// <summary>
/// Данные для тестирования аутентификации пользователя для некорректных входных данных.
/// </summary>
public class IncorrectDataTestData : TheoryData<string, string>
{
    public IncorrectDataTestData()
    {
        Add($"user_{Guid.NewGuid()}", short.MaxValue.ToString());
        Add(string.Empty, short.MaxValue.ToString());
        Add(" ", short.MaxValue.ToString());
        Add($"user_{Guid.NewGuid()}@mail.ru", " ");
    }
}
