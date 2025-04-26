using System.Security.Cryptography;

namespace IdentityWebApp.Services;

/// <summary>
/// Сервис криптографии.
/// </summary>
public static class CryptographyService
{
    #region Методы

    /// <summary>
    /// Генерирует безопасный ключ заданного размера для использования с HMAC.
    /// </summary>
    /// <param name="size">Размер ключа в байтах. Должен быть положительным числом. По умолчанию 64 байта (512 бит).</param>
    /// <returns>Ключ в виде строки Base64.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Возникает, если размер ключа меньше или равен нулю.</exception>
     public static string GenerateSecureKey(int size = 64)
     {
        if (size <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(size), ConstantsService.KeySizeOutOfRangeErrorMessage);
        }

        var keyBytes = new byte[size];

        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(keyBytes);
        }

        return Convert.ToBase64String(keyBytes);
    }

    #endregion
}
