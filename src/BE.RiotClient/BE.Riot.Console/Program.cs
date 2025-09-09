using BE.Riot;
using BE.Riot.Console;
using Microsoft.Extensions.DependencyInjection;


var services = Startup.ConfigureServices();
var provider = services.BuildServiceProvider();
var client = provider.GetRequiredService<IRiotClient>();

Console.Write("Summoner Name:");
var summonerName = Console.ReadLine();

Console.Write("TagLine:");
var tagLine = Console.ReadLine();

var puuid = await client.GetPuuIdBy(summonerName, tagLine);

if (!puuid.Found)
{
    Console.WriteLine("User not found");
    return;   
}
var matches = await client.GetLatestMatchIdsByPuuId(puuid.Puiid.Value);
Console.WriteLine("Puuid: " + puuid.Puiid.Value);