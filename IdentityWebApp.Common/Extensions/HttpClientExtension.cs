using System.Text;
using IdentityWebApp.Common.Services;

namespace IdentityWebApp.Common.Extensions;

/// <summary>
/// Расширения для <see cref="HttpClient"/>.
/// </summary>
public static class HttpClientExtension
{
	/// <summary>
    /// Асинхронно отправляет POST-запрос с объектом в формате JSON и получает ответ в виде объекта.
    /// </summary>
    /// <typeparam name="TInput">Тип объекта, который будет отправлен в запросе.</typeparam>
    /// <typeparam name="TOutput">Тип объекта, который будет получен в ответе.</typeparam>
    /// <param name="client">Экземпляр HttpClient, используемый для отправки запроса.</param>
    /// <param name="url">URL-адрес, на который будет отправлен запрос.</param>
    /// <param name="inputObject">Объект, который будет сериализован и отправлен в запросе.</param>
    /// <param name="mediaType">Тип медиа-контента.</param>
    /// <returns>Асинхронная задача, возвращающая десериализованный объект ответа или null в случае ошибки.</returns>
	public static async Task<TOutput?> PostAsync<TInput, TOutput>(this HttpClient client, string url, 
		TInput inputObject, string mediaType = "application/json")
	{		
		var content = await JsonSerializationService.SerializeAsync(inputObject);
		var contentAsString = new StringContent(content, Encoding.UTF8, mediaType);

		var response = await client.PostAsync(url, contentAsString);
		response.EnsureSuccessStatusCode();

        var responseContentAsString = await response.Content.ReadAsStringAsync();

		return await JsonSerializationService.DeserializeAsync<TOutput>(responseContentAsString);
	}
}
