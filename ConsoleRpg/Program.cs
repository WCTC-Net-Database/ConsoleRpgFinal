// The code below is commented out for now, but will be used in a future lesson.
// It demonstrates Dependency Injection (DI) using Microsoft's built-in DI container.
// DI is a key part of the "D" in SOLID (Dependency Inversion Principle).
// With DI, we let a container create and provide our dependencies, making our code more flexible and testable.
// For now, we are using manual instantiation (see below), but soon you'll see how DI can simplify and decouple this process.

/*
using ConsoleRpg.Decorators;
using ConsoleRpg.Helpers;
using ConsoleRpg.Services;
using ConsoleRpgEntities.Data;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleRpg;

public static class Program
{
    private static void Main(string[] args)
    {
        // Setup DI container
        var serviceCollection = new ServiceCollection();
        Startup.ConfigureServices(serviceCollection);
        var serviceProvider = serviceCollection.BuildServiceProvider();

        // Get the GameEngine and run it
        var gameEngine = serviceProvider.GetService<GameEngine>();
        gameEngine?.Run();
    }
}
*/

// -----------------------------------------------------------------------------
// CURRENT APPROACH: Manual Instantiation (No Dependency Injection)
// -----------------------------------------------------------------------------
// In this version, we create ("new up") all the objects ourselves, and pass their
// dependencies directly into their constructors. This is called "concrete instantiation."
// Each class is responsible for knowing exactly what it needs and how to get it.
//
// This approach is simple and easy to follow for small projects or when learning.
// However, as projects grow, it can become harder to manage and test, because
// dependencies are "embedded" (hard-wired) into the code.
//
// Later, we'll switch to the DI approach above, which will:
//   - Move the responsibility for creating objects to a container
//   - Make it easier to swap out implementations (for example, for testing)
//   - Reduce coupling between classes
//   - Demonstrate the Dependency Inversion Principle (the "D" in SOLID)
//
// For now, focus on understanding how dependencies are passed and used!

using ConsoleRpg;
using ConsoleRpg.Decorators;
using ConsoleRpg.Helpers;
using ConsoleRpg.Services;
using ConsoleRpgEntities.Data;

namespace ConsoleRpg;

public static class Program
{
    private static void Main(string[] args)
    {
        // Manually create each dependency, from the "bottom" up.
        // Notice how each object is constructed with the objects it needs.
        var context = new GameContext();
        var outputManager = new OutputManager();
        var menuManager = new MenuManager(outputManager);
        var playerDao = new PlayerDao(context);
        var playerService = new PlayerService(outputManager, playerDao);

        // Decorate the player service with auto-save functionality.
        // This wraps the playerService so that changes are automatically saved.
        var autoSavePlayerService = new AutoSavePlayerServiceDecorator(playerService, context);

        // Pass all dependencies to the GameEngine.
        // This is called "constructor injection" (even though we're doing it manually).
        var gameEngine = new GameEngine(context, menuManager, outputManager, autoSavePlayerService);

        // Start the game!
        gameEngine.Run();
    }
}

// -----------------------------------------------------------------------------
// SUMMARY
// -----------------------------------------------------------------------------
// - The code above and the commented-out DI code do the same thing: they create
//   all the objects needed for the game and connect them together.
// - The difference is *how* the dependencies are provided:
//     - Here, we "hard-wire" (embed) the dependencies by hand.
//     - With DI, a container manages dependencies, making the code more flexible.
// - Later, you'll see how DI helps us follow the Dependency Inversion Principle
//   (the "D" in SOLID), making our code easier to change, test, and extend.
