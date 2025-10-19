using BE.CQRS.Domain.Conventions;
using BE.CQRS.Domain.DomainObjects;
using BE.Riot.Events;
using BE.Riot.MatchHistories;
using Microsoft.Extensions.Logging;

namespace BE.Riot.EventSource;

public sealed class PlayerMatchHistoryDomainObject(string id, IMatchHistoryUpdater matchHistoryUpdater, ILogger<PlayerMatchHistoryDomainObject> logger) : DomainObjectBase(id)
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
        var matches = await matchHistoryUpdater.UpdateSinceLatestStoredAsync(this.Id, CancellationToken.None);

        logger.LogDebug("{count} matches read", matches.Count);
        RaiseEvent<SummonerMatchesRead>(x =>
            x.MatchIds = matches.ToHashSet());
    }
}