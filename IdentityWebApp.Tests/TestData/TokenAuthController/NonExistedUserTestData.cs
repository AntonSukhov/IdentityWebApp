namespace IdentityWebApp.Tests.TestData.TokenAuthController;

/// <summary>
/// Данные для тестирования аутентификации несуществующего пользователя.
/// </summary>
public class NonExistedUserTestData : TheoryData<string, string>
{
    public NonExistedUserTestData()
    {
        Add($"user_{Guid.NewGuid()}", "1vh54kS4QlHDADF40o0MWRN1V////iZx/OcTavXqsMt2SIfWYlM2yS+bA+5g6q5R");
        Add(string.Empty, "hNFVWrW3MYZqN98TDwMxhEW1YqShkXiE2mkNZvAaBjQ=");
        Add(" ", "uhmwYKcmumKYjzXt55v2+A/IK62XxF2NG0aIJL3AeKI=");
    }
}
