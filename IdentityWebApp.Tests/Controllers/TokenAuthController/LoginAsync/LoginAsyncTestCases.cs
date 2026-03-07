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
    private const string Email = "my_test_email@mail.ru";
    private const string Password = "Qwerty";

    /// <summary>
    /// Получает сценарии успешного выполнения метода 
    /// <see cref="IdentityWebApp.Controllers.TokenAuthController.LoginAsync"/>.
    /// </summary>
    public static TheoryData<TestCaseInputWithStubs<UserLoginModel>> ExistedUserTestCases
    {
        get
        {
            var theoryData = new TheoryData<TestCaseInputWithStubs<UserLoginModel>>
            {
                new() {
                    ScenarioNumber = 1,
                    Description = "Проверка успешной выдачи токена при попытке авторизации существующего пользователя.",
                    InputData = new UserLoginModel 
                    { 
                        Login = Email, 
                        Password = Password
                    }, 
                    StubOutputs = new Dictionary<StubOutputKey, StubOutput>
                    {
                        [new StubOutputKey(UserManagerMethodNames.FindByNameAsync, 
                            StubSequenceConstants.First)] = new StubOutput
                        {
                            OutputData =  new ApplicationUser 
                            { 
                                Id = Guid.NewGuid().ToString(), 
                                Email = Email, 
                                UserName = Email 
                            },
                            ExpectedType = typeof(ApplicationUser)
                        }
                    }
                }
            };

            return theoryData;
        }
    }
    
    /// <summary>
    /// Получает сценарии выполнения метода 
    /// <see cref="IdentityWebApp.Controllers.TokenAuthController.LoginAsync"/> 
    /// для не существующих пользователей системы.
    /// </summary>
    public static TheoryData<TestCaseInput<UserLoginModel>> UnknownUserTestCases
    {
        get
        {
            var theoryData = new TheoryData<TestCaseInput<UserLoginModel>>
            {
                new() {
                    ScenarioNumber = 1,
                    Description = "Проверка отсутствия выдачи токена при попытке авторизации несуществующего пользователя.",
                    InputData = new UserLoginModel 
                    { 
                        Login = $"user_{Guid.NewGuid()}", 
                        Password = Password
                    }
                },
                new() {
                    ScenarioNumber = 2,
                    Description = "Проверка отсутствия выдачи токена при попытке авторизации несуществующего пользователя с "+
                                    "пустым логином.",
                    InputData = new UserLoginModel 
                    { 
                        Login = string.Empty, 
                        Password = Password
                    }
                },
                new() {
                    ScenarioNumber = 3,
                    Description = "Проверка отсутствия выдачи токена при попытке авторизации несуществующего пользователя с "+
                                "логином равным пробелу.",
                    InputData = new UserLoginModel 
                    { 
                        Login = " ", 
                        Password = Password
                    }
                }
            };
            
            return theoryData;
        }
    }
    
    /// <summary>
    /// Получает сценарии выполнения метода 
    /// <see cref="IdentityWebApp.Controllers.TokenAuthController.LoginAsync"/> 
    /// для пользователя системы с пустым или содержащим только пробелы паролем. 
    /// </summary>
    public static TheoryData<TestCaseInputWithStubs<UserLoginModel>> EmptyOrWhitespacePasswordTestCases
    {
        get
        {
            var theoryData = new TheoryData<TestCaseInputWithStubs<UserLoginModel>>
            {
                new() {
                    ScenarioNumber = 1,
                    Description = "Проверка отсутствия выдачи токена при попытке авторизации пользователя с пустым паролем.",
                    InputData = new UserLoginModel 
                    { 
                        Login = Email, 
                        Password = string.Empty
                    },
                    StubOutputs =new Dictionary<StubOutputKey, StubOutput>
                    {
                        [new StubOutputKey(UserManagerMethodNames.FindByNameAsync, 
                            StubSequenceConstants.First)] = new StubOutput
                        {
                            OutputData =  new ApplicationUser 
                            { 
                                Id = Guid.NewGuid().ToString(), 
                                Email = Email, 
                                UserName = Email 
                            },
                            ExpectedType = typeof(ApplicationUser)
                        }
                    }
                },
                new() {
                    ScenarioNumber = 2,
                    Description = "Проверка отсутствия выдачи токена при попытке авторизации пользователя с паролем содержащим "+ 
                                "только пробелы.",
                    InputData = new UserLoginModel 
                    { 
                        Login = Email, 
                        Password = "  "
                    },
                    StubOutputs =new Dictionary<StubOutputKey, StubOutput>
                    {
                        [new StubOutputKey(UserManagerMethodNames.FindByNameAsync, 
                            StubSequenceConstants.First)] = new StubOutput
                        {
                            OutputData =  new ApplicationUser 
                            { 
                                Id = Guid.NewGuid().ToString(), 
                                Email = Email, 
                                UserName = Email 
                            },
                            ExpectedType = typeof(ApplicationUser)
                        }
                    }
                }
            };

            return theoryData;
        }
    }
    
    /// <summary>
    /// Получает сценарии выполнения метода 
    /// <see cref="IdentityWebApp.Controllers.TokenAuthController.LoginAsync"/> 
    /// для существующего пользователя системы с коротким временем жизни токена. 
    /// </summary>
    public static TheoryData<TestCaseInputWithStubs<UserContext>> ExistedUserWithShortTokenLifetimeTestCases
    {
        get
        {
            var theoryData = new TheoryData<TestCaseInputWithStubs<UserContext>>
            {
                new() {
                    ScenarioNumber = 1,
                    Description = "Проверка выдачи токена для существующего пользователя с коротким временем жизни токена " + 
                                "равным 1 секунде.",
                    InputData = new UserContext 
                    { 
                        UserLogin = new UserLoginModel
                        {
                            Login = Email, 
                            Password =  Password
                        },
                        TokenLifetimeSeconds = 1
                    },
                    StubOutputs =new Dictionary<StubOutputKey, StubOutput>
                    {
                        [new StubOutputKey(UserManagerMethodNames.FindByNameAsync, 
                            StubSequenceConstants.First)] = new StubOutput
                        {
                            OutputData =  new ApplicationUser 
                            { 
                                Id = Guid.NewGuid().ToString(), 
                                Email = Email, 
                                UserName = Email 
                            },
                            ExpectedType = typeof(ApplicationUser)
                        }
                    }
                },
                new() {
                    ScenarioNumber = 2,
                    Description = "Проверка выдачи токена для существующего пользователя с коротким временем жизни токена " +
                                "равным 3 секундам.",
                    InputData = new UserContext 
                    { 
                        UserLogin = new UserLoginModel
                        {
                            Login = Email, 
                            Password = Password
                        },
                        TokenLifetimeSeconds = 3
                    },
                    StubOutputs =new Dictionary<StubOutputKey, StubOutput>
                    {
                        [new StubOutputKey(UserManagerMethodNames.FindByNameAsync, 
                            StubSequenceConstants.First)] = new StubOutput
                        {
                            OutputData =  new ApplicationUser 
                            { 
                                Id = Guid.NewGuid().ToString(), 
                                Email = Email, 
                                UserName = Email 
                            },
                            ExpectedType = typeof(ApplicationUser)
                        }
                    }
                },
                new() {
                    ScenarioNumber = 3,
                    Description = "Проверка выдачи токена для существующего пользователя с коротким временем жизни токена " +
                                "равным 5 секундам.",
                    InputData = new UserContext 
                    { 
                        UserLogin = new UserLoginModel
                        {
                            Login = Email, 
                            Password = Password
                        },
                        TokenLifetimeSeconds = 5
                    },
                    StubOutputs =new Dictionary<StubOutputKey, StubOutput>
                    {
                        [new StubOutputKey(UserManagerMethodNames.FindByNameAsync, 
                            StubSequenceConstants.First)] = new StubOutput
                        {
                            OutputData =  new ApplicationUser 
                            { 
                                Id = Guid.NewGuid().ToString(), 
                                Email = Email, 
                                UserName = Email 
                            },
                            ExpectedType = typeof(ApplicationUser)
                        }
                    }
                }
            };

            return theoryData;
        }
    }
   
    /// <summary>
    /// Получает сценарии выполнения метода 
    /// <see cref="IdentityWebApp.Controllers.TokenAuthController.LoginAsync"/> 
    ///  для существующего пользователя с временем жизни токена, большем чем 
    ///  период времени между повторными запросами. 
    /// </summary>
    public static TheoryData<TestCaseInputWithStubs<UserLoginModel>> 
        ExistedUserTokenLifetimeExceedsRequestIntervalTestCases
    {
        get
        {
            var theoryData = new TheoryData<TestCaseInputWithStubs<UserLoginModel>>
            {
                new() {
                    ScenarioNumber = 1,
                    Description = "Проверка выдачи одного и того-же токена для существующего пользователя с временем жизни токена" + 
                                "большем чем период времени между повторными запросами",
                    InputData = new UserLoginModel
                    {
                        Login = Email, 
                        Password = Password
                    },
                    StubOutputs = new Dictionary<StubOutputKey, StubOutput>
                    {
                        [new StubOutputKey(UserManagerMethodNames.FindByNameAsync, 
                            StubSequenceConstants.First)] = new StubOutput
                        {
                            OutputData =  new ApplicationUser 
                            { 
                                Id = Guid.NewGuid().ToString(), 
                                Email = Email, 
                                UserName = Email
                            },
                            ExpectedType = typeof(ApplicationUser)
                        }
                    }
                }
            };

            return theoryData;
        }
    }
}