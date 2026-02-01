using IdentityWebApp.Areas.Identity.Models;

namespace IdentityWebApp.Tests.Controllers.TokenAuthController.LoginAsync;

public class UserContext
{
    public UserLoginModel UserLogin { get; set; }
    public int TokenLifetimeSeconds { get; set; }

    public UserContext()
    {
        UserLogin = new UserLoginModel { Login = string.Empty, Password = string.Empty };
    }
}
