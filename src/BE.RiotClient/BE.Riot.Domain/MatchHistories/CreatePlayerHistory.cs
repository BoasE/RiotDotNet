using BE.CQRS.Domain.Commands;

namespace BE.Riot.EventSource;

public sealed class CreatePlayerHistory : CommandBase
{
    public string SummonerName { get; set; }
    public string TagLine { get; set; }
    public string PuuId { get; set; }
    public CreatePlayerHistory(string domainObjectId) : base(domainObjectId)
    {
    }
}