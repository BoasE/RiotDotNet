namespace BE.Riot.MatchHistories;

public interface ISummonerMatchHistoryIdsGateway
{
    Task Add(string puuid, string matchId, DateTimeOffset timestamp);
    Task<DateTimeOffset?> LatestMatchDateTime(string puuId, CancellationToken cancellationToken);
    Task<DateTimeOffset?> OldestMatchDateTime(string puuId, CancellationToken cancellationToken);
}