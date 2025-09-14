using ConsoleRpgEntities.Models;
using Microsoft.EntityFrameworkCore;

namespace ConsoleRpgEntities.Data
{
    public interface IContext
    {
        List<Player> Players { get; set; }
        void Read();
        void Write(Player player);
        int SaveChanges();
    }
}
