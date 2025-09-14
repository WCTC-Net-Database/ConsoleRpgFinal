using ConsoleRpgEntities.Models;
using System.Collections.Generic;

// 1. DAO Handles Data Access Only
// •	PlayerDao.Add(Player) should add to context (via Write), but should not call SaveChanges().
// •	The DAO should not be called directly from the decorator.
public interface IPlayerDao
{
    void Add(Player player);
    Player GetByName(string name);
    List<Player> GetAll();
    // Add more methods as needed (e.g., Remove, Update, etc.)
}
