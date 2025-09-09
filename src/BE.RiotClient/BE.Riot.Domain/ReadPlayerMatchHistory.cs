using BE.CQRS.Domain.Commands;

namespace BE.Riot;

public sealed class ReadPlayerMatchHistory : CommandBase
{

    public ReadPlayerMatchHistory(string domainObjectId) : base(domainObjectId)
    {
    }
}