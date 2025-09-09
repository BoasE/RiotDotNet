using BE.Riot.Http;

namespace BE.Riot;

public interface IRiotClient
{
    Task<ISet<MatchId>> GetLatestMatchIdsByPuuId(string puuId,int count = 50);

    Task<GetPuuIdResult> GetPuuIdBy(string summonerName, string tagName);
}