using BE.CQRS.Domain.Conventions;
using BE.Riot.Events;
using BE.Riot.MatchHistories;

namespace BE.Riot.EventSource.Denormalizer;

[Denormalizer]
public sealed class MatchHistoryDenormalizer
{
    private readonly ISummonerMatchHistoryIdsGateway gtw;
    public MatchHistoryDenormalizer(ISummonerMatchHistoryIdsGateway gtw)
    {
        this.gtw = gtw;
    }
    public async void On(SummonerMatchesRead @event)
    {
        foreach (var entry in @event.MatchIds)
        {
            await gtw.Add(@event.Headers.AggregateId, entry, DateTimeOffset.UtcNow);
        }
    }
}