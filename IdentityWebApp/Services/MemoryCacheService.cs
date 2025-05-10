using Microsoft.Extensions.Caching.Memory;

namespace IdentityWebApp.Services;

/// <summary>
/// Реализация обобщенного кэша в пямяти.
/// </summary>
/// <typeparam name="TKey">Тип ключа для кэша.</typeparam>
/// <typeparam name="TValue">Тип значения для кэша.</typeparam>
public class MemoryCacheService<TKey, TValue> : ICacheService<TKey, TValue>
{
    #region Поля

    private readonly IMemoryCache _memoryCache;

    #endregion

    #region Конструкторы

    /// <summary>
    /// Конструктор, принимающий экземпляр кэша в памяти.
    /// </summary>
    /// <param name="memoryCache">Экземпляр кэша в памяти.</param>
    public MemoryCacheService(IMemoryCache memoryCache)
    {
        ArgumentNullException.ThrowIfNull(memoryCache);

        _memoryCache = memoryCache;
    }

    #endregion
    
    #region Методы

    /// <inheritdoc/>
    public void Set(TKey key, TValue value, DateTimeOffset absoluteExpiration)
    {     
        _memoryCache.Set(key, value, absoluteExpiration);
    }

    /// <inheritdoc/>
    public bool TryGetValue(TKey key, out TValue value)
    {
        return _memoryCache.TryGetValue(key, out value);
    }
    
    /// <inheritdoc/>
    public void Remove(TKey key)
    {
        _memoryCache.Remove(key);
    }

    #endregion
}
