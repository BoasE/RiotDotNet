using BE.CQRS.Domain;
using BE.CQRS.Domain.Commands;
using BE.Riot;
using BE.Riot.Console;
using BE.Riot.EventSource;
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

var repo = provider.GetRequiredService<IDomainObjectRepository>();
var bus = provider.GetRequiredService<ICommandBus>();
if (!await repo.Exists<PlayerMatchHistoryDomainObject>(puuid.Puiid.Value))
{
    await bus.EnqueueAsync(new CreatePlayerHistory(puuid.Puiid.Value)
    {
        SummonerName = summonerName,
        TagLine = tagLine,
        PuuId = puuid.Puiid.Value
    });
}

await bus.EnqueueAsync(new ReadPlayerMatchHistory(puuid.Puiid.Value));
Console.WriteLine("Puuid: " + puuid.Puiid.Value);