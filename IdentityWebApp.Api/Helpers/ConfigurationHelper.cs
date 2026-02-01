using System.Xml.Linq;

namespace IdentityWebApp.Api.Helpers;

/// <summary>
/// Помощник в работе с конфигурациями.
/// </summary>
public static class ConfigurationHelper
{
    /// <summary>
    /// Получает идентификатор пользовательских секретов из файла проекта.
    /// </summary>
    /// <returns>Идентификатор пользовательских секретов или null.</returns>
    public static string? GetUserSecretsId()
    {
        var projectFullPath = GetCurrentProjectFullPath();

        if (string.IsNullOrWhiteSpace(projectFullPath))
        {
            return null;
        }

        try
        {
            var doc = XDocument.Load(projectFullPath);
            var userSecretsId = doc.Descendants("UserSecretsId")
                                   .FirstOrDefault()?.Value;

            return userSecretsId;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Находит полный путь к файлу .csproj текущего проекта.
    /// </summary>
    /// <returns>
    /// Полный путь к файлу .csproj, если найден, иначе null.
    /// </returns>
    /// <remarks>
    /// Поиск выполняется вверх по дереву каталогов, начиная от директории исполняемого файла.
    /// </remarks>
    private static string? GetCurrentProjectFullPath()
    {
        var currentDirectory = AppContext.BaseDirectory;

        while (currentDirectory != null)
        {
            var projectFile = Directory.EnumerateFiles(currentDirectory, "*.csproj")
                                       .FirstOrDefault();

            if (projectFile != null)
            {
                return projectFile;
            }

            currentDirectory = Directory.GetParent(currentDirectory)?.FullName;
        }

        return null;
    }
}
