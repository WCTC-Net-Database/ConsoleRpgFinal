using ConsoleRpgEntities.Models;

namespace ConsoleRpg.Services;

public interface IPlayerService
{
    void LevelUpPlayer(Player player);
    void AddPlayer(Player player);
    List<Player> GetAllPlayers();
}