namespace ConsoleRpgEntities.Interfaces
{
    public interface IMonster
    {
        string Name { get; set; }
        int Health { get; set; }

        void TakeDamage(int amount);
        int DealDamage();
    }
}
