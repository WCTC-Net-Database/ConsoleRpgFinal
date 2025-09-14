namespace ConsoleRpgEntities.Models
{
    /// <summary>
    /// Represents a player character in the RPG game.
    /// 
    /// This class is a pure data model (entity) and should not contain any business logic,
    /// persistence, or cross-cutting concerns. It is used for data storage, transfer, and display.
    /// 
    /// SRP: Only represents the state and properties of a player.
    /// </summary>
    public class Player
    {
        public string Name { get; set; }
        public string Profession { get; set; }
        public int Level { get; set; }
        public int HitPoints { get; set; }
        public List<string> Equipment { get; set; }

        /// <summary>
        /// Default constructor. Initializes the Equipment list.
        /// </summary>
        public Player()
        {
            Equipment = new List<string>();
            // must be explicitly defined if a parameterized constructor is present
        }

        /// <summary>
        /// Parameterized constructor for creating a player with all properties set.
        /// </summary>
        /// <param name="name">Player's name</param>
        /// <param name="profession">Player's profession/class</param>
        /// <param name="level">Player's level</param>
        /// <param name="hitPoints">Player's hit points</param>
        /// <param name="equipment">List of equipment items</param>
        public Player(string name, string profession, int level, int hitPoints, List<string> equipment)
        {
            Name = name;
            Profession = profession;
            Level = level;
            HitPoints = hitPoints;
            Equipment = equipment;
        }

        /// <summary>
        /// Returns a string representation of the player's state for display purposes.
        /// </summary>
        override public string ToString()
        {
            string equipmentList = Equipment != null && Equipment.Count > 0 ? string.Join(", ", Equipment) : "None";
            return $"Name: {Name}\nProfession: {Profession}\nLevel: {Level}\nHit Points: {HitPoints}\nEquipment: {equipmentList}";
        }
    }
}
