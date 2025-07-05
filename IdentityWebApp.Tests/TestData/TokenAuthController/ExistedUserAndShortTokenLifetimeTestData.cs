namespace IdentityWebApp.Tests.TestData.TokenAuthController;

/// <summary>
/// Данные для тестирования аутентификации существующего пользователя с малым временем жизни токена.
/// </summary>
public class ExistedUserAndShortTokenLifetimeTestData : TheoryData<string, string, int>
{
    public ExistedUserAndShortTokenLifetimeTestData()
    {
        Add("my_test_email_1@gmail.com", "uLm3+lVgPM7RnNnuTA5vOmQpAM9dOK4FJ6cCJ9+FoPqwkyWni58g+yR0fEGPe0OG", 1);
        Add("my_test_email_2@gmail.com", "x+5zXDe3ttID3BGJjlreW+QpTPAzQRtfKPCBtlCCkBufzffWWEAsjEGmM5yjcLJY", 3);
        Add("my_test_email_3@yandex.ru", "fa4WnjyMCDCg6ELNMEDWcO8LJJbzuMHfAhCQCTiHtlfSnp3CKMxlm0BnoCVYJJyf", 5);
    }
}
