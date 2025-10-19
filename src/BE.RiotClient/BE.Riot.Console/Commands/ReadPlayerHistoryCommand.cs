using BE.CQRS.Domain;
using BE.CQRS.Domain.Commands;
using BE.Riot.EventSource;
using BE.Riot.Http;
using BE.Riot.MatchHistories;

namespace BE.Riot.Console.Commands;

public interface IReadPlayerHistoryCommand
{
    Task<string> Execute(string summonerName, string tag);
}

public sealed class ReadPlayerHistoryCommand(IDomainObjectRepository repo, IRiotClient client, ICommandBus bus) : IReadPlayerHistoryCommand
{
    public async Task<string> Execute(string summonerName, string tag)
    {
        GetPuuIdResult response = await client.GetPuuIdBy(summonerName, tag);
        var puuid = response.Puiid;
        if (!await repo.Exists<PlayerMatchHistoryDomainObject>(puuid))
        {
            await bus.EnqueueAsync(new CreatePlayerHistory(puuid)
            {
                SummonerName = summonerName,
                TagLine = tag,
                PuuId = puuid
            });
        }

        var cmd = new ReadPlayerMatchHistory(puuid);

        await bus.EnqueueAsync(cmd);
        return puuid.Value;
    }
}