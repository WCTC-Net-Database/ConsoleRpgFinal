using ConsoleRpg.Decorators;
using ConsoleRpg.Helpers;
using ConsoleRpg.Services;
using ConsoleRpgEntities.Data;
using ConsoleRpgEntities.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NReco.Logging.File;

namespace ConsoleRpg;

public static class Startup
{
    public static void ConfigureServices(IServiceCollection services)
    {
        // Build configuration
        var configuration = ConfigurationHelper.GetConfiguration();

        // Create and bind FileLoggerOptions
        var fileLoggerOptions = new FileLoggerOptions();
        configuration.GetSection("Logging:File").Bind(fileLoggerOptions);

        // Configure logging
        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddConfiguration(configuration.GetSection("Logging"));

            // Add Console logger
            loggingBuilder.AddConsole();

            // Add File logger using the correct constructor
            var logFileName = "Logs/log.txt"; // Specify the log file path

            loggingBuilder.AddProvider(new FileLoggerProvider(logFileName, fileLoggerOptions));
        });

        // Register your services
        services.AddSingleton<IOutputService, OutputService>();
        services.AddSingleton<IContext, GameContext>();
        services.AddSingleton<OutputManager>();
        services.AddSingleton<MenuManager>();

        // Register DAO
        services.AddSingleton<IPlayerDao, PlayerDao>();

        // Register PlayerService and the decorator for IPlayerService.
        //
        // We register the concrete PlayerService so it can be wrapped by the decorator.
        // Then, we register IPlayerService as an instance of AutoSavePlayerServiceDecorator,
        // passing in the concrete PlayerService and IContext.
        //
        // This ensures that when IPlayerService is injected elsewhere (e.g., GameEngine),
        // the decorator is used, wrapping the real PlayerService. This avoids recursion:
        // _inner in the decorator is always the concrete PlayerService, not another decorator.
        //
        // Example:
        //   IPlayerService (injected) --> AutoSavePlayerServiceDecorator
        //                                         |
        //                                         v
        //                                PlayerService (concrete)
        services.AddSingleton<PlayerService>();
        services.AddSingleton<IPlayerService>(provider =>
            new AutoSavePlayerServiceDecorator(
                provider.GetRequiredService<PlayerService>(),
                provider.GetRequiredService<IContext>()));



        services.AddSingleton<GameEngine>();
    }
}
