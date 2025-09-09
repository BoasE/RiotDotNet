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

    public static string MatchesIdByPuuid(string host, string puuId)
    {
        return string.Concat(host, MatchesPath, "by-puuid/", puuId, "/ids");
    }
}