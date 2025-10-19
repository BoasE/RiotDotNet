using BE.CQRS.Domain.Commands;

namespace BE.Riot.MatchHistories;

public sealed class ReadPlayerMatchHistory : CommandBase
{

    public ReadPlayerMatchHistory(string domainObjectId) : base(domainObjectId)
    {
    }
}