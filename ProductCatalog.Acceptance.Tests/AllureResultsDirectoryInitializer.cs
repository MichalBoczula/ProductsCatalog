using System.Runtime.CompilerServices;
using System.Text.Json;

namespace ProductCatalog.Acceptance.Tests;

internal static class AllureResultsDirectoryInitializer
{
    [ModuleInitializer]
    internal static void Initialize()
    {
        var configPath = Path.Combine(AppContext.BaseDirectory, "allureConfig.json");

        if (!File.Exists(configPath))
        {
            return;
        }

        using var document = JsonDocument.Parse(File.ReadAllText(configPath));

        if (!document.RootElement.TryGetProperty("allure", out var allureConfig))
        {
            return;
        }

        if (!allureConfig.TryGetProperty("directory", out var directoryElement))
        {
            return;
        }

        var relativeDirectory = directoryElement.GetString();

        if (string.IsNullOrWhiteSpace(relativeDirectory))
        {
            return;
        }

        var resolvedDirectory = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, relativeDirectory));

        Directory.CreateDirectory(resolvedDirectory);
    }
}
