using Microsoft.Extensions.Configuration;

namespace ConsoleRpg.Helpers;

public static class ConfigurationHelper
{
    public static IConfigurationRoot GetConfiguration(string basePath = null, string environmentName = null)
    {
        // Use the directory of the main project (ConsoleRpg) as the base path
        basePath ??= AppContext.BaseDirectory;

        var builder = new ConfigurationBuilder()
            .SetBasePath(basePath) // Look for appsettings.json in ConsoleRpg/bin/Debug/net8.0
            .AddJsonFile("appsettings.json", false, true);

        // Optionally add environment-specific configuration
        if (!string.IsNullOrEmpty(environmentName))
        {
            builder.AddJsonFile($"appsettings.{environmentName}.json", true);
        }

        return builder.Build();
    }
}
