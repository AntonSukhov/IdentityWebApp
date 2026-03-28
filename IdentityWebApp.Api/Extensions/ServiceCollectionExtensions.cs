using IdentityWebApp.Api.Services;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityWebApp.Api.Extensions;

// <summary>
/// Расширения для <see cref="IServiceCollection"/> для регистрации сервисов IdentityWebApp.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Добавляет сервис аутентификации пользователей для IdentityWebApp API.
    /// </summary>
    /// <param name="services">Коллекция сервисов DI‑контейнера.</param>
    /// <param name="serviceLifetime">Время жизни сервиса.</param>
    /// <returns>Коллекция сервисов для поддержки цепочки вызовов.</returns>
    public static IServiceCollection AddIdentityWebAppAuthentication(
        this IServiceCollection services,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        var serviceDescriptor = new ServiceDescriptor(
            typeof(IAuthenticationService),
            typeof(AuthenticationService),
            serviceLifetime
        );

        services.Add(serviceDescriptor);

        return services;
    }
}
