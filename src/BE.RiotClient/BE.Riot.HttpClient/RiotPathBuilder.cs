namespace BE.Riot.Http;

public static class RiotPathBuilder
{
    private const string AccountPath = "riot/account/v1/accounts/";
    private const string PuuidBySummonerPath = AccountPath + "by-riot-id/";

    private const string MatchesPath = "lol/match/v5/matches/";

    public static string PuiidBySummonerName(string host, string summonerName, string tagName)
    {
        return string.Concat(host, PuuidBySummonerPath, summonerName, "/", tagName);
    }

    public static string MatchesIdByPuuid(
        string host,
        string puuId,
        int? count,
        int? start,
        DateTimeOffset? startTime,
        DateTimeOffset? endTime,
        int? queue,
        string? type)
    {
        var basePath = string.Concat(host, MatchesPath, "by-puuid/", Uri.EscapeDataString(puuId), "/ids");

        var query = new List<string>();
        if (startTime.HasValue) query.Add($"startTime={startTime.Value.ToUnixTimeSeconds()}");
        if (endTime.HasValue) query.Add($"endTime={endTime.Value.ToUnixTimeSeconds()}");
        if (queue.HasValue) query.Add($"queue={queue.Value}");
        if (!string.IsNullOrWhiteSpace(type)) query.Add($"type={Uri.EscapeDataString(type)}");
        if (start.HasValue) query.Add($"start={start.Value}");
        if (count.HasValue) query.Add($"count={count.Value}");

        return query.Count == 0 ? basePath : $"{basePath}?{string.Join("&", query)}";
    }

    public static string? MatchById(string host, string matchId)
    {
        string url = string.Concat(host, MatchesPath, matchId);

        return url;
    }
}