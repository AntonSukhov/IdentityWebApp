using IdentityWebApp.Data;
using Microsoft.AspNetCore.Identity;

namespace IdentityWebApp.Tests.TestSupport.Constants;

/// <summary>
/// Набор константных строк с именами методов <see cref="UserManager{TUser}"/> для использования в тестах.
/// </summary>
public static class UserManagerMethodNames
{
    /// <summary>
    /// Имя метода <see cref="UserManager{TUser}.FindByNameAsync"/>.
    /// </summary>
    public const string FindByNameAsync = nameof(UserManager<ApplicationUser>.FindByNameAsync);

    /// <summary>
    /// Имя метода <see cref="UserManager{TUser}.CheckPasswordAsync"/>.
    /// </summary>
    public const string CheckPasswordAsync = nameof(UserManager<ApplicationUser>.CheckPasswordAsync);

    /// <summary>
    /// Имя метода <see cref="UserManager{TUser}.GetRolesAsync"/>.
    /// </summary>
    public const string GetRolesAsync = nameof(UserManager<ApplicationUser>.GetRolesAsync);
}
