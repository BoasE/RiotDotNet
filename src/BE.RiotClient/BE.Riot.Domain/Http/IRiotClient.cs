using BE.Riot.Http;
using BE.Riot.MatchHistories;

namespace BE.Riot;

public interface IRiotClient
{
    Task<ISet<MatchId>> GetLatestMatchIdsByPuuId(
        string puuId,
        int? count = null,
        int? start = null,
        DateTimeOffset? startTime = null,
        DateTimeOffset? endTime = null,
        int? queue = null,
        string? type = null);

    Task<GetPuuIdResult> GetPuuIdBy(string summonerName, string tagName);

    /// <see href="https://developer.riotgames.com/apis#match-v5/GET_getMatch"/>
    Task<CompletedGame?> GetMatchById(string matchId);
}