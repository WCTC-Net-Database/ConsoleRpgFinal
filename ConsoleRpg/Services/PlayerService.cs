using ConsoleRpg.Helpers;
using ConsoleRpgEntities.Models;

namespace ConsoleRpg.Services;

/// <summary>
/// PlayerService contains business logic and logging for Player operations.
/// It should not perform data access directly or handle persistence.
/// 
/// SRP: Only business logic and logging for Player.
/// </summary>
public class PlayerService : IPlayerService
{
    private readonly OutputManager _outputManager;
    private readonly IPlayerDao _playerDao;

    public PlayerService(OutputManager outputManager, IPlayerDao playerDao)
    {
        _outputManager = outputManager;
        _playerDao = playerDao;
    }

    public void LevelUpPlayer(Player player)
    {
        player.Level++;
        player.HitPoints += 5;
        _outputManager.AddLogEntry($"Player {player.Name} leveled up to level {player.Level} with {player.HitPoints} hit points.");
    }

    public void AddPlayer(Player player)
    {
        _playerDao.Add(player);
        _outputManager.AddLogEntry($"Player {player.Name} was added.");
    }

    public List<Player> GetAllPlayers()
    {
        return _playerDao.GetAll();
    }
}
