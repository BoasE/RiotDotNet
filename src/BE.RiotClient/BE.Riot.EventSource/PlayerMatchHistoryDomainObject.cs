using BE.CQRS.Domain.Conventions;
using BE.CQRS.Domain.DomainObjects;
using BE.Riot.Events;
using Microsoft.Extensions.Logging;

namespace BE.Riot.EventSource;

public sealed class PlayerMatchHistoryDomainObject(string id, IRiotClient client, ILogger<PlayerMatchHistoryDomainObject> logger) : DomainObjectBase(id)
{
    private readonly ILogger<PlayerMatchHistoryDomainObject> logger = logger;

    [Create]
    public void On(CreatePlayerHistory cmd)
    {
        RaiseEvent<PlayerMatchHistoryCreated>(x =>
        {
            x.SummonerName = cmd.SummonerName;
            x.TagLine = cmd.TagLine;
            x.PuuId = cmd.PuuId;
        });
    }

    [UpdateWithoutHistory]
    public async Task On(ReadPlayerMatchHistory cmd)
    {
        var matches = await client.GetLatestMatchIdsByPuuId(cmd.DomainObjectId,99);

        logger.LogDebug("{count} matches read", matches.Count);
        RaiseEvent<SummonerMatchesRead>(x =>
            x.MatchIds = matches.Select(x => x.Value).ToHashSet());
    }
}