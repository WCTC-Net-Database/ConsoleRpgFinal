namespace ConsoleRpgEntities.Interfaces
{
    public interface IAbility
    {
        string Name { get; }
        void Execute(IPlayer user, IMonster target);
    }
}
