using BE.CQRS.Domain.Events;

namespace BE.Riot.Events;

public sealed class SummonerMatchesRead : EventBase
{
    public HashSet<string> MatchIds
    {
        get;
        set;
    }
}