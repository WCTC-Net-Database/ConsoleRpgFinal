using ConsoleRpgEntities.Models;
using CsvHelper;
using CsvHelper.Configuration;
using System.Formats.Asn1;
using System.Globalization;

/// <summary>
/// GameContext is responsible for data storage and persistence.
/// It implements IContext and should only handle reading/writing data,
/// not business logic or cross-cutting concerns.
/// 
/// SRP: Only manages data access and persistence.
/// </summary>
namespace ConsoleRpgEntities.Data
{
    public class GameContext : IContext
    {
        public List<Player> Players { get; set; }

        private string _fileName = "Files/input.csv";

        public GameContext()
        {
            Players = new List<Player>();
            Read();
        }

        /// <summary>
        /// Loads players from the CSV file into memory.
        /// </summary>
        public void Read()
        {
            using (var reader = new StreamReader(_fileName))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<PlayerMap>();
                Players = csv.GetRecords<Player>().ToList();
            }
        }

        /// <summary>
        /// Adds a player to the in-memory list.
        /// Does not persist to disk until SaveChanges is called.
        /// </summary>
        public void Write(Player player)
        {
            Players.Add(player);
        }

        /// <summary>
        /// Persists all players to the CSV file.
        /// </summary>
        public int SaveChanges()
        {
            using (var writer = new StreamWriter(_fileName))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<PlayerMap>();
                csv.WriteHeader<Player>();
                csv.NextRecord();
                foreach (var character in Players)
                {
                    csv.WriteRecord(character);
                    csv.NextRecord();
                }
            }
            return Players.Count;
        }

        // CsvHelper mapping for Player
        private sealed class PlayerMap : ClassMap<Player>
        {
            public PlayerMap()
            {
                Map(m => m.Name);
                Map(m => m.Profession);
                Map(m => m.Level);
                Map(m => m.HitPoints);
                // Read: CSV to object
                Map(m => m.Equipment)
                    .Convert(args =>
                    {
                        var field = args.Row.GetField("Equipment");
                        return string.IsNullOrEmpty(field)
                            ? new List<string>()
                            : field.Split('|').ToList();
                    })
                    .Name("Equipment")
                    // Write: object to CSV
                    .Convert(args =>
                    {
                        var equipment = args.Value.Equipment ?? new List<string>();
                        return string.Join("|", equipment);
                    });
            }
        }


    }
}
