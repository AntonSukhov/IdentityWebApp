using System;

namespace IdentityWebApp.Tests.TestData.TokenAuthController;

/// <summary>
/// Данные для тестирования аутентификации существующего пользователя с малым временем жизни токена.
/// </summary>
public class ExistedUserAndShortTokenLifetimeTestData : TheoryData<string, string, int>
{
    public ExistedUserAndShortTokenLifetimeTestData()
    {
        Add("my_test_email_1@gmail.com", "my_test_password_89101", 1);
        Add("my_test_email_2@gmail.com", "my_test_password_1901", 3);
        Add("my_test_email_3@yandex.ru", "my_test_password_7891", 5);
    }
}
