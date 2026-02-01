using IdentityWebApp.Services.Senders;
using Infrastructure.Caching.Services;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace IdentityWebApp.Extensions;

/// <summary>
/// Расширение функционала объекта <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Регистрация сервисов в коллекцию сервисов.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddSingleton<ICacheService<string, string>, MemoryCacheService<string, string>>();
        services.AddTransient<IEmailSender, SmtpEmailSender>();
    }

}
