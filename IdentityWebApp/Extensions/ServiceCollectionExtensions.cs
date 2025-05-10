using IdentityWebApp.Services;
using IdentityWebApp.Services.Senders;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace IdentityWebApp.Extensions;

/// <summary>
/// Расширение функционала объекта <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    #region Методы

    /// <summary>
    /// Регистрация сервисов в коллекцию сервисов.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddSingleton<ICacheService<string, string>, MemoryCacheService<string, string>>();
        services.AddTransient<IEmailSender, SmtpEmailSender>();
    }

    #endregion
}
