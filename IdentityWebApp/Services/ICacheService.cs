namespace IdentityWebApp.Services;

/// <summary>
/// Сервис работы с обобщенным кэшем.
/// </summary>
/// <typeparam name="TKey">Тип ключа для кэша.</typeparam>
/// <typeparam name="TValue">Тип значения для кэша.</typeparam>
public interface ICacheService<in TKey, TValue>
{
    #region Методы

    /// <summary>
    /// Устанавливает значение в кэш с указанным ключом и временем жизни.
    /// </summary>
    /// <param name="key">Ключ для хранения значения.</param>
    /// <param name="value">Значение, которое нужно сохранить.</param>
    /// <param name="absoluteExpiration">Момент времени, в который истекает срок действия записи в кэше.</param>
    void Set(TKey key, TValue value, DateTimeOffset absoluteExpiration);

    /// <summary>
    /// Пытается получить значение из кэша по указанному ключу.
    /// </summary>
    /// <param name="key">Ключ для поиска значения.</param>
    /// <param name="value">Выходное значение, если оно найдено.</param>
    /// <returns>True, если значение найдено; иначе - false.</returns>
    bool TryGetValue(TKey key, out TValue value);

    /// <summary>
    /// Удаляет значение из кэша по указанному ключу.
    /// </summary>
    /// <param name="key">Ключ для удаления значения.</param>
    void Remove(TKey key);

    #endregion
}
