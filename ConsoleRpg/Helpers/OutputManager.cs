using ConsoleRpgEntities.Models;
using Spectre.Console;

namespace ConsoleRpg.Helpers;

public class OutputManager
{
    private const int MaxLogLines = 15; // Maximum number of visible log lines
    private readonly Layout _layout;
    private readonly List<string> _logContent;

    public OutputManager(string initialMapContent = "### Initial Map Area ###")
    {
        _logContent = new List<string>();

        // Create the layout with two regions
        _layout = new Layout()
            .SplitRows(
                new Layout("Map").Size(10), // Fixed size for the map area
                new Layout("Logs")); // Flexible size for the log area

        // Set the initial content for each region
        _layout["Logs"].Update(CreateLogPanel());
    }

    public void AddLogEntry(string logEntry)
    {
        // Add the log entry
        _logContent.Add(logEntry);

        // If the log exceeds the maximum visible lines, scroll
        var visibleLogs = _logContent.Skip(Math.Max(0, _logContent.Count - MaxLogLines)).ToList();

        // Update the Logs region of the layout
        _layout["Logs"].Update(CreateLogPanel(visibleLogs));

        // Re-render the layout
        //AnsiConsole.Clear();
        AnsiConsole.Cursor.SetPosition(0, 0);
        AnsiConsole.Write(_layout);
    }

    public string GetUserInput(string prompt)
    {
        // Add the prompt to the visible logs for display
        var visibleLogs = _logContent.Skip(Math.Max(0, _logContent.Count - MaxLogLines)).ToList();
        visibleLogs.Add($"[yellow]{Markup.Escape(prompt)}[/]");

        // Update the Logs region with the prompt
        _layout["Logs"].Update(CreateLogPanel(visibleLogs));

        // Re-render the layout without clearing the console
        AnsiConsole.Write(_layout);

        // Move the cursor to the end of the log panel
        var cursorTop = Console.CursorTop;
        Console.SetCursorPosition(2, cursorTop); // 2 spaces for padding after the border

        // Display the input prompt and capture user input
        Console.Write("> ");
        var userInput = Console.ReadLine()?.Trim() ?? string.Empty;

        // Log the user's input
        AddLogEntry($"[yellow]User Input:[/] {Markup.Escape(userInput)}");

        return userInput;
    }

    private Panel CreateLogPanel(IEnumerable<string> logs = null)
    {
        // Create a panel for the logs
        var logText = string.Join("\n", logs ?? _logContent);

        return new Panel(new Markup(logText))
            .Expand()
            .Border(BoxBorder.Square)
            .Padding(1, 1, 1, 1)
            .Header($"Logs ({_logContent.Count})");
    }

    public void ShowGoodbyeModal()
    {
        AnsiConsole.Clear();

        // Big FigletText for fun
        AnsiConsole.Write(
            new FigletText("Goodbye!")
                .Centered()
                .Color(Color.Green1)
        );

        // Modal-style panel
        var goodbyePanel = new Panel(
                new Markup("[bold yellow]Thanks for playing![/]\n\n[green]May your adventures continue elsewhere...[/]\n")
            )
            .Header("[bold green]Farewell[/]")
            .Border(BoxBorder.Double)
            .BorderStyle(new Style(Color.Green1))
            .Padding(2, 1)
            .Expand();

        AnsiConsole.Write(goodbyePanel);

        // Optional: Pause for a moment so the user can see the message
        AnsiConsole.Status()
            .Start("Exiting...", ctx =>
            {
                Thread.Sleep(1500);
            });
    }

    public void ShowPlayerTable(IEnumerable<Player> players, string title = "Players")
    {
        var table = new Table()
            .Border(TableBorder.Rounded)
            .Title($"[bold yellow]{title}[/]")
            .AddColumn("[green]#[/]")
            .AddColumn("[cyan]Name[/]")
            .AddColumn("[magenta]Profession[/]")
            .AddColumn("[blue]Level[/]")
            .AddColumn("[red]HP[/]")
            .AddColumn("[grey]Equipment[/]");

        int i = 1;
        foreach (var p in players)
        {
            table.AddRow(
                i.ToString(),
                Markup.Escape(p.Name),
                Markup.Escape(p.Profession),
                p.Level.ToString(),
                p.HitPoints.ToString(),
                Markup.Escape(string.Join(", ", p.Equipment ?? new List<string>()))
            );
            i++;
        }

        var panel = new Panel(table)
            .Header($"[bold]{title}[/]")
            .Border(BoxBorder.Square)
            .Expand();

        // Update the Logs region of the layout with the player table
        _layout["Logs"].Update(panel);

        // Re-render the layout
        AnsiConsole.Cursor.SetPosition(0, 0);
        AnsiConsole.Write(_layout);
    }


    public string PromptInput(string prompt)
    {
        return AnsiConsole.Prompt(
            new TextPrompt<string>($"[yellow]{Markup.Escape(prompt)}[/]")
                .PromptStyle("green")
                .AllowEmpty()
        );
    }

}
