using IdentityWebApp.Areas.Identity.Models;
using IdentityWebApp.Data;
using IdentityWebApp.Tests.TestSupport.Constants;
using Infrastructure.Testing.Common;
using Infrastructure.Testing.TestCases;

namespace IdentityWebApp.Tests.Controllers.TokenAuthController.LoginAsync;

/// <summary>
/// Набор тестовых сценариев для проверки метода 
/// <see cref="IdentityWebApp.Controllers.TokenAuthController.LoginAsync"/>.
/// </summary>
public static class LoginAsyncTestCases
{
    private const string Email1 = "my_test_email@mail.ru";
    private const string Email2 = "my_test_email_1@gmail.com";
    private const string Email3 = "my_test_email_2@gmail.com";
    private const string Email4 = "my_test_email_3@yandex.ru";
    private const string Email5 = "email@mail.ru";
    private const string Password = "arkznDPAeHXqx8TNL4dZqbXUJl4L66LLL2+SXSgfuNM=";

    /// <summary>
    /// Получает сценарии успешного выполнения метода 
    /// <see cref="IdentityWebApp.Controllers.TokenAuthController.LoginAsync"/>.
    /// </summary>
    public static TheoryData<TestCaseInputWithStubs<UserLoginModel>> 
        ExistedUserTestCases =>
        [
            new TestCaseInputWithStubs<UserLoginModel>
            {
                ScenarioNumber = 1,
                Description = "Проверка успешной выдачи токена при попытке авторизации существующего пользователя.",
                InputData = new UserLoginModel 
                { 
                    Login = Email1, 
                    Password = Password
                }, 
                StubOutputs =new Dictionary<(string MethodName, int SequenceNumber), StubOutput>
                {
                    [(UserManagerMethodNames.FindByNameAsync, 
                        StubSequenceConstants.First)] = new StubOutput
                    {
                        OutputData =  new ApplicationUser 
                        { 
                            Id = Guid.NewGuid().ToString(), 
                            Email = Email1, 
                            UserName = Email1 
                        },
                        ExpectedType = typeof(ApplicationUser)
                    }
                }
            }
        ];

    /// <summary>
    /// Получает сценарии выполнения метода 
    /// <see cref="IdentityWebApp.Controllers.TokenAuthController.LoginAsync"/> 
    /// для не существующих пользователей системы.
    /// </summary>
    public static TheoryData<TestCaseInput<UserLoginModel>> 
        UnknownUserTestCases =>
        [
            new TestCaseInput<UserLoginModel>
            {
                ScenarioNumber = 1,
                Description = "Проверка отсутствия выдачи токена при попытке авторизации несуществующего пользователя.",
                InputData = new UserLoginModel 
                { 
                    Login = $"user_{Guid.NewGuid()}", 
                    Password = "1vh54kS4QlHDADF40o0MWRN1V////iZx/OcTavXqsMt2SIfWYlM2yS+bA+5g6q5R"
                }
            },
            new TestCaseInput<UserLoginModel>
            {
                ScenarioNumber = 2,
                Description = "Проверка отсутствия выдачи токена при попытке авторизации несуществующего пользователя с пустым логином.",
                InputData = new UserLoginModel 
                { 
                    Login = string.Empty, 
                    Password = "hNFVWrW3MYZqN98TDwMxhEW1YqShkXiE2mkNZvAaBjQ="
                }
            },
            new TestCaseInput<UserLoginModel>
            {
                ScenarioNumber = 3,
                Description = "Проверка отсутствия выдачи токена при попытке авторизации несуществующего пользователя с "+
                              "логином равным пробелу.",
                InputData = new UserLoginModel 
                { 
                    Login = " ", 
                    Password = "uhmwYKcmumKYjzXt55v2+A/IK62XxF2NG0aIJL3AeKI="
                }
            }
        ];

    /// <summary>
    /// Получает сценарии выполнения метода 
    /// <see cref="IdentityWebApp.Controllers.TokenAuthController.LoginAsync"/> 
    /// для пользователя системы с пустым или содержащим только пробелы паролем. 
    /// </summary>
    public static TheoryData<TestCaseInputWithStubs<UserLoginModel>> 
        EmptyOrWhitespacePasswordTestCases =>
        [
            new TestCaseInputWithStubs<UserLoginModel>
            {
                ScenarioNumber = 1,
                Description = "Проверка отсутствия выдачи токена при попытке авторизации пользователя с пустым паролем.",
                InputData = new UserLoginModel 
                { 
                    Login = Email1, 
                    Password = string.Empty
                },
                StubOutputs =new Dictionary<(string MethodName, int SequenceNumber), StubOutput>
                {
                    [(UserManagerMethodNames.FindByNameAsync, 
                        StubSequenceConstants.First)] = new StubOutput
                    {
                        OutputData =  new ApplicationUser 
                        { 
                            Id = Guid.NewGuid().ToString(), 
                            Email = Email1, 
                            UserName = Email1 
                        },
                        ExpectedType = typeof(ApplicationUser)
                    }
                }
            },
            new TestCaseInputWithStubs<UserLoginModel>
            {
                ScenarioNumber = 2,
                Description = "Проверка отсутствия выдачи токена при попытке авторизации пользователя с паролем содержащим "+ 
                              "только пробелы.",
                InputData = new UserLoginModel 
                { 
                    Login = Email1, 
                    Password = "  "
                },
                StubOutputs =new Dictionary<(string MethodName, int SequenceNumber), StubOutput>
                {
                    [(UserManagerMethodNames.FindByNameAsync, 
                        StubSequenceConstants.First)] = new StubOutput
                    {
                        OutputData =  new ApplicationUser 
                        { 
                            Id = Guid.NewGuid().ToString(), 
                            Email = Email1, 
                            UserName = Email1 
                        },
                        ExpectedType = typeof(ApplicationUser)
                    }
                }
            }
        ];

    /// <summary>
    /// Получает сценарии выполнения метода 
    /// <see cref="IdentityWebApp.Controllers.TokenAuthController.LoginAsync"/> 
    /// для существующего пользователя системы с коротким временем жизни токена. 
    /// </summary>
    public static TheoryData<TestCaseInputWithStubs<UserContext>> 
        ExistedUserWithShortTokenLifetimeTestCases =>
        [
            new TestCaseInputWithStubs<UserContext>
            {
                ScenarioNumber = 1,
                Description = "Проверка выдачи токена для существующего пользователя с коротким временем жизни токена " + 
                              "равным 1 секунде.",
                InputData = new UserContext 
                { 
                    UserLogin = new UserLoginModel
                    {
                        Login = Email2, 
                        Password = "uLm3+lVgPM7RnNnuTA5vOmQpAM9dOK4FJ6cCJ9+FoPqwkyWni58g+yR0fEGPe0OG"
                    },
                    TokenLifetimeSeconds = 1
                },
                StubOutputs =new Dictionary<(string MethodName, int SequenceNumber), StubOutput>
                {
                    [(UserManagerMethodNames.FindByNameAsync, 
                        StubSequenceConstants.First)] = new StubOutput
                    {
                        OutputData =  new ApplicationUser 
                        { 
                            Id = Guid.NewGuid().ToString(), 
                            Email = Email2, 
                            UserName = Email2 
                        },
                        ExpectedType = typeof(ApplicationUser)
                    }
                }
            },
             new TestCaseInputWithStubs<UserContext>
            {
                ScenarioNumber = 2,
                Description = "Проверка выдачи токена для существующего пользователя с коротким временем жизни токена " +
                              "равным 3 секундам.",
                InputData = new UserContext 
                { 
                    UserLogin = new UserLoginModel
                    {
                        Login = Email3, 
                        Password = "x+5zXDe3ttID3BGJjlreW+QpTPAzQRtfKPCBtlCCkBufzffWWEAsjEGmM5yjcLJY"
                    },
                    TokenLifetimeSeconds = 3
                },
                StubOutputs =new Dictionary<(string MethodName, int SequenceNumber), StubOutput>
                {
                    [(UserManagerMethodNames.FindByNameAsync, 
                        StubSequenceConstants.First)] = new StubOutput
                    {
                        OutputData =  new ApplicationUser 
                        { 
                            Id = Guid.NewGuid().ToString(), 
                            Email = Email3, 
                            UserName = Email3 
                        },
                        ExpectedType = typeof(ApplicationUser)
                    }
                }
            },
             new TestCaseInputWithStubs<UserContext>
            {
                ScenarioNumber = 3,
                Description = "Проверка выдачи токена для существующего пользователя с коротким временем жизни токена " +
                              "равным 5 секундам.",
                InputData = new UserContext 
                { 
                    UserLogin = new UserLoginModel
                    {
                        Login = Email4, 
                        Password = "fa4WnjyMCDCg6ELNMEDWcO8LJJbzuMHfAhCQCTiHtlfSnp3CKMxlm0BnoCVYJJyf"
                    },
                    TokenLifetimeSeconds = 5
                },
                StubOutputs =new Dictionary<(string MethodName, int SequenceNumber), StubOutput>
                {
                    [(UserManagerMethodNames.FindByNameAsync, 
                        StubSequenceConstants.First)] = new StubOutput
                    {
                        OutputData =  new ApplicationUser 
                        { 
                            Id = Guid.NewGuid().ToString(), 
                            Email = Email4, 
                            UserName = Email4 
                        },
                        ExpectedType = typeof(ApplicationUser)
                    }
                }
            }
        ];

    /// <summary>
    /// Получает сценарии выполнения метода 
    /// <see cref="IdentityWebApp.Controllers.TokenAuthController.LoginAsync"/> 
    ///  для существующего пользователя с временем жизни токена, большем чем 
    ///  период времени между повторными запросами. 
    /// </summary>
    public static TheoryData<TestCaseInputWithStubs<UserLoginModel>> 
        ExistedUserTokenLifetimeExceedsRequestIntervalTestCases =>
        [
            new TestCaseInputWithStubs<UserLoginModel>
            {
                ScenarioNumber = 1,
                Description = "Проверка выдачи одного и того-же токена для существующего пользователя с временем жизни токена" + 
                              "большем чем период времени между повторными запросами",
                InputData = new UserLoginModel
                {
                    Login = Email5, 
                    Password = "RV3wuhfDkzkD8GezQ5e4PwrVibHT9zErY+mIW69M5uE="
                },
                StubOutputs =new Dictionary<(string MethodName, int SequenceNumber), StubOutput>
                {
                    [(UserManagerMethodNames.FindByNameAsync, 
                        StubSequenceConstants.First)] = new StubOutput
                    {
                        OutputData =  new ApplicationUser 
                        { 
                            Id = Guid.NewGuid().ToString(), 
                            Email = Email5, 
                            UserName = Email5 
                        },
                        ExpectedType = typeof(ApplicationUser)
                    }
                }
            }
        ];
}