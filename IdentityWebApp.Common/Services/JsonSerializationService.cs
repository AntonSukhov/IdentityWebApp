using System.Text;
using System.Text.Json;

namespace IdentityWebApp.Common.Services;

/// <summary>
/// Сервис, предоставляющий методы для преобразования объектов в формат JSON и обратно.
/// </summary>
public static class JsonSerializationService
{
	/// <summary>
    /// Асинхронно сериализует объект в строку формата JSON.
    /// </summary>
    /// <typeparam name="TValue">Тип объекта для сериализации.</typeparam>
    /// <param name="value">Объект, который нужно сериализовать.</param>
    /// <param name="options">Опции сериализации.</param>
    /// <param name="cancellationToken">Токен отмены для асинхронной операции.</param>
    /// <returns>Асинхронная задача, возвращающая строку в формате JSON.</returns>
	public static async Task<string> SerializeAsync<TValue>(TValue value, JsonSerializerOptions? options = null, 
		CancellationToken cancellationToken = default)
	{
		using var memoryStream = new MemoryStream();

		await JsonSerializer.SerializeAsync(memoryStream, value, options, cancellationToken);

		memoryStream.Position = 0L;

		using var reader = new StreamReader(memoryStream, Encoding.UTF8);

		return await reader.ReadToEndAsync(cancellationToken);
	}

    /// <summary>
    /// Асинхронно десериализует строку в формате JSON в объект указанного типа.
    /// </summary>
    /// <typeparam name="TValue">Тип объекта для десериализации.</typeparam>
    /// <param name="json">Строка в формате JSON, которую нужно десериализовать.</param>
    /// <param name="options">Опции десериализации.</param>
    /// <param name="cancellationToken">Токен отмены для асинхронной операции.</param>
    /// <returns>Асинхронная задача, возвращающая десериализованный объект.</returns>
    /// <exception cref="JsonException">Выбрасывается, если результат десериализации равен null.</exception>
	public static async Task<TValue> DeserializeAsync<TValue>(string json, JsonSerializerOptions? options = null, 
		CancellationToken cancellationToken = default)
	{
		var jsonAsBytes = Encoding.UTF8.GetBytes(json);
		using var memoryStream = new MemoryStream(jsonAsBytes);

		var value = await JsonSerializer.DeserializeAsync<TValue>(memoryStream, options, cancellationToken);

        return value ?? throw new JsonException("Ошибка десериализации JSON: результат десериализации равен null.");
	}
}
