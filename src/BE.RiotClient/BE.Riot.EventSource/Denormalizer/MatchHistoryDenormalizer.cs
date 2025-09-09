using BE.CQRS.Domain.Conventions;
using BE.Riot.Events;

namespace BE.Riot.EventSource.Denormalizer;

[Denormalizer]
public sealed class MatchHistoryDenormalizer
{
    public void On(PlayerMatchHistoryCreated @event)
    {
    }
    
}