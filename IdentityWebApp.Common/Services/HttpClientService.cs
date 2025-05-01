using Microsoft.Extensions.DependencyInjection;

namespace IdentityWebApp.Common.Services;

/// <summary>
/// Сервис для создания и настройки Http-клиента.
/// </summary>
public static class HttpClientService
{
    #region Методы

    /// <summary>
    /// Создаёт и настраивает экземпляр Http-клиента.
    /// </summary>
    /// <returns>Экземпляр Http-клиента, готовый к использованию.</returns>
    /// <exception cref="InvalidOperationException">Выбрасывается, если фабрика Http-клиентов не зарегистрирована
    /// в контейнере зависимостей.</exception>
    public static HttpClient CreateHttpClient()
    {
        var services = new ServiceCollection();
        services.AddHttpClient();

        var serviceProvider = services.BuildServiceProvider();
        
        var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();

        if (httpClientFactory == null)
        {
            throw new InvalidOperationException($"{nameof(httpClientFactory)} не зарегистрирован в {nameof(services)}.");
        }

        var httpClient = httpClientFactory.CreateClient();

        return httpClient;
    }

    #endregion
}
