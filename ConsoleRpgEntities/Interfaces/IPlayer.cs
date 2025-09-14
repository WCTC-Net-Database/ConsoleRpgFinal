namespace ConsoleRpgEntities.Interfaces
{
    public interface IPlayer
    {
        string Name { get; set; }
        int Health { get; set; }
        int Attack { get; set; }
        int Defense { get; set; }

        void TakeDamage(int amount);
        void AttackEnemy(IMonster monster);
    }
}
