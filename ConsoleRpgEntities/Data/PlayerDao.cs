using ConsoleRpgEntities.Data;
using ConsoleRpgEntities.Models;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// PlayerDao is the Data Access Object for Player entities.
/// It abstracts all data access operations for Player, using IContext as the backend.
/// 
/// SRP: Only handles data access for Player. No business logic or persistence triggers.
/// </summary>
public class PlayerDao : IPlayerDao
{
    private readonly IContext _context;

    public PlayerDao(IContext context)
    {
        _context = context;
    }

    public void Add(Player player)
    {
        _context.Write(player);
    }

    public Player GetByName(string name)
    {
        return _context.Players.FirstOrDefault(p => p.Name == name);
    }

    public List<Player> GetAll()
    {
        return _context.Players.ToList();
    }
}
