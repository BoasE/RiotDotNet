using BE.CQRS.Domain.Events;

namespace BE.Riot.Events;

public sealed class PlayerMatchHistoryCreated : EventBase
{
    public string SummonerName { get; set; }
    public string TagLine { get; set; }
    public string PuuId { get; set; }
}