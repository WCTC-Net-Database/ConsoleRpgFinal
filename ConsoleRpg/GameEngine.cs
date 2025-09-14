using ConsoleRpg.Helpers;
using ConsoleRpg.Services;
using ConsoleRpgEntities.Data;
using ConsoleRpgEntities.Models;

namespace ConsoleRpg;

/// <summary>
/// GameEngine orchestrates the game flow and user interaction.
/// It should only use high-level services and never interact with data access or persistence directly.
/// 
/// SRP: Only responsible for game orchestration and user interaction.
/// </summary>
public class GameEngine
{
    private readonly MenuManager _menuManager;
    private readonly OutputManager _outputManager;

    private readonly IPlayerService _playerService;
    private Player _player;

    public GameEngine(IContext context, MenuManager menuManager, OutputManager outputManager, IPlayerService playerService)
    {
        _menuManager = menuManager;
        _outputManager = outputManager;
        _playerService = playerService;
    }

    public void Run()
    {
        if (_menuManager.ShowMainMenu())
        {
            SetupGame();
        }
    }

    private void GameLoop()
    {
        while (true)
        {
            _outputManager.AddLogEntry("1. Level Up Player");
            _outputManager.AddLogEntry("2. Add Player");
            _outputManager.AddLogEntry("3. List All Players");
            _outputManager.AddLogEntry("0. Quit");
            var input = _outputManager.GetUserInput("Choose an action:");

            switch (input)
            {
                case "1":
                    _outputManager.AddLogEntry("Leveling up player...");
                    _playerService.LevelUpPlayer(_player);
                    break;
                case "2":
                    var newPlayer = PromptForNewPlayer();
                    if (newPlayer != null)
                    {
                        _playerService.AddPlayer(newPlayer);
                    }
                    break;
                case "3":
                    ListAllPlayers();
                    break;
                case "0":
                    _outputManager.ShowGoodbyeModal();
                    Environment.Exit(0);
                    break;
                default:
                    _outputManager.AddLogEntry("Invalid selection. Please choose 1.");
                    break;
            }
        }
    }

    private void SetupGame()
    {
        _player = SelectPlayer();
        if (_player == null)
        {
            _outputManager.AddLogEntry("[red]No player selected. Exiting game setup.[/]");
            Environment.Exit(0);
        }
        _outputManager.AddLogEntry($"{_player.Name} has entered the game.");

        // Pause before starting the game loop
        Thread.Sleep(500);
        GameLoop();
    }

    /// <summary>
    /// Prompts the user for new player details using OutputManager.
    /// All input validation and parsing is handled here.
    /// </summary>
    private Player PromptForNewPlayer()
    {
        var name = _outputManager.GetUserInput("Enter player name:");
        var profession = _outputManager.GetUserInput("Enter profession:");
        var levelStr = _outputManager.GetUserInput("Enter level:");
        var hitPointsStr = _outputManager.GetUserInput("Enter hit points:");
        var equipmentStr = _outputManager.GetUserInput("Enter equipment (comma or | separated):");

        if (!int.TryParse(levelStr, out var level) || !int.TryParse(hitPointsStr, out var hitPoints))
        {
            _outputManager.AddLogEntry("[red]Invalid level or hit points. Player not added.[/]");
            return null;
        }

        var equipment = equipmentStr
            .Split(new[] { ',', '|' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(e => e.Trim())
            .ToList();

        return new Player(name, profession, level, hitPoints, equipment);
    }

    private Player SelectPlayer()
    {
        var players = _playerService.GetAllPlayers();
        if (players == null || players.Count == 0)
        {
            _outputManager.AddLogEntry("[red]No players found. Please add a player first.[/]");
            return null;
        }

        _outputManager.ShowPlayerTable(players, "Available Players");

        while (true)
        {
            var input = _outputManager.PromptInput("Select a player by number:");
            if (int.TryParse(input, out int index) && index > 0 && index <= players.Count)
            {
                return players[index - 1];
            }
            _outputManager.AddLogEntry("[red]Invalid selection. Please enter a valid number.[/]");
        }
    }


    private void ListAllPlayers()
    {
        var players = _playerService.GetAllPlayers();
        if (players == null || players.Count == 0)
        {
            _outputManager.AddLogEntry("[red]No players found.[/]");
            return;
        }
        _outputManager.ShowPlayerTable(players, "All Players");
        _outputManager.PromptInput("Press [green]Enter[/] to return to the menu...");
    }



}
