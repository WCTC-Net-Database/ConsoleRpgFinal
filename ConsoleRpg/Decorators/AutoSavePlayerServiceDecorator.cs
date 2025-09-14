using ConsoleRpg.Services;
using ConsoleRpgEntities.Data;
using ConsoleRpgEntities.Models;

namespace ConsoleRpg.Decorators;

/// <summary>
/// AutoSavePlayerServiceDecorator is an example of the "Decorator Pattern".
/// 
/// What is a Decorator?
/// --------------------
/// A decorator "wraps" another object that implements the same interface (here, IPlayerService),
/// allowing us to add extra behavior before or after the original object's methods are called,
/// without changing the original class. This is a classic way to follow the Open/Closed Principle (O in SOLID).
///
/// What does this decorator do?
/// ---------------------------
/// - It wraps any IPlayerService (such as PlayerService).
/// - After any method that changes data (like AddPlayer or LevelUpPlayer), it calls SaveChanges() on the context,
///   ensuring that changes are persisted to storage automatically.
/// - For read-only methods (like GetAllPlayers), it just passes the call through with no extra behavior.
///
/// How is this decorator used in this project?
/// -------------------------------------------
/// - In Program.cs (manual instantiation), we create a PlayerService, then wrap it with this decorator:
///     var playerService = new PlayerService(outputManager, playerDao);
///     var autoSavePlayerService = new AutoSavePlayerServiceDecorator(playerService, context);
///   The GameEngine then uses autoSavePlayerService for all player operations.
/// - This means: whenever the game adds or levels up a player, the decorator ensures changes are saved,
///   without the GameEngine or PlayerService needing to know about persistence details.
///
/// How does this relate to dependency injection?
/// ---------------------------------------------
/// - In the future, with Dependency Injection (DI), the DI container will create and "wrap" these services for us.
/// - For now, we do it by hand in Program.cs, but the pattern is the same: the GameEngine only knows about IPlayerService,
///   not which specific implementation (or decorator) it is using.
/// - This makes the code flexible and easy to extend or test (for example, you could add a LoggingPlayerServiceDecorator
///   in the same way, without changing GameEngine or PlayerService).
///
/// Why is this good design?
/// ------------------------
/// - Follows the Single Responsibility Principle (SRP): PlayerService handles business logic, this decorator handles persistence.
/// - Follows the Open/Closed Principle (OCP): We can add new behaviors (like auto-saving) without modifying existing code.
/// - Supports the Dependency Inversion Principle (DIP): GameEngine depends on abstractions (IPlayerService), not concrete classes.
/// </summary>
public class AutoSavePlayerServiceDecorator : IPlayerService
{
    // The "inner" service being decorated (usually PlayerService)
    private readonly IPlayerService _service;
    // The context used to persist changes
    private readonly IContext _context;

    /// <summary>
    /// Constructor: takes the service to decorate and the context for saving changes.
    /// In manual instantiation (see Program.cs), these are passed in directly.
    /// With DI, the container would provide them.
    /// </summary>
    public AutoSavePlayerServiceDecorator(IPlayerService service, IContext context)
    {
        _service = service;
        _context = context;
    }

    /// <summary>
    /// When a player is leveled up, first call the original service's method,
    /// then save changes to persist the update.
    /// </summary>
    public void LevelUpPlayer(Player player)
    {
        _service.LevelUpPlayer(player);
        _context.SaveChanges(); // Automatically save after mutation
    }

    /// <summary>
    /// When a player is added, first call the original service's method,
    /// then save changes to persist the new player.
    /// </summary>
    public void AddPlayer(Player player)
    {
        _service.AddPlayer(player);       // Call business logic/logging
        _context.SaveChanges();           // Persist
    }

    /// <summary>
    /// For read-only operations, just pass through to the original service.
    /// No need to save changes.
    /// </summary>
    public List<Player> GetAllPlayers()
    {
        return _service.GetAllPlayers(); // No save needed for queries
    }
}
