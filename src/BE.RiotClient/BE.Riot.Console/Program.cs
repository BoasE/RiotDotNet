using BE.Riot.Console;
using BE.Riot.Console.Commands;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;
using Spectre.Console.Extensions;

var services = Startup.ConfigureServices();
var provider = services.BuildServiceProvider();

var summonerName = AnsiConsole.Ask<string>("Summoner Name:");
var tagLine = AnsiConsole.Ask<string>("TagLine: #");
tagLine = tagLine.Replace("#", "");

var cmd = provider.GetRequiredService<IReadPlayerHistoryCommand>();

string? puuid = null;

await AnsiConsole.Status()
    .StartAsync("Loading player data...", async ctx =>
    {
        puuid = await cmd.Execute(summonerName, tagLine).Spinner();
    });

AnsiConsole.MarkupLine($"[green]Puuid:[/] {puuid}");