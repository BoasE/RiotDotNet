using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using BE.Riot.MatchHistories;

namespace BE.Riot.Http;

public sealed class MatchResponse
{
    public MatchMetadata? Metadata { get; set; }
    public MatchInfo? Info { get; set; }
    public CompletedGame? ToEntity()
    {
        CompletedGame result;
        try
        {
            result = new CompletedGame(Metadata.MatchId, Info?.Participants?.Where(x => x != null && !string.IsNullOrWhiteSpace(x.Puuid)).Select(x => x.ToEntity()).ToList(), Info.EndOfGameResult, Info.GameCreation, Info.GameDuration, Info.GameEndTimestamp, Info.GameId,
                Info.GameMode, Info.GameName, Info.GameStartTimestamp, Info.GameType, Info.GameVersion, Info.MapId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return result;
    }
}

public sealed class MatchMetadata
{
    public string? DataVersion { get; set; }
    public string? MatchId { get; set; }
    public List<string>? Participants { get; set; }
}

public sealed class MatchInfo
{
    public string? EndOfGameResult { get; set; }
    public long? GameCreation { get; set; }
    public int? GameDuration { get; set; }
    public long? GameEndTimestamp { get; set; }
    public long? GameId { get; set; }
    public string? GameMode { get; set; }
    public string? GameName { get; set; }
    public long? GameStartTimestamp { get; set; }
    public string? GameType { get; set; }
    public string? GameVersion { get; set; }
    public int? MapId { get; set; }
    public List<MatchParticipant>? Participants { get; set; }

    // Unerfasste Felder werden hier abgelegt, damit die Deserialisierung robust bleibt
    [JsonExtensionData]
    public Dictionary<string, JsonElement>? ExtensionData { get; set; }
}

public sealed class MatchParticipant
{
    public string? Puuid { get; set; }
    public int? ParticipantId { get; set; }
    public int? TeamId { get; set; }
    public bool? Win { get; set; }

    public int? ChampionId { get; set; }
    public string? ChampionName { get; set; }
    public int? ChampLevel { get; set; }

    public string? IndividualPosition { get; set; }
    public string? TeamPosition { get; set; }
    public string? Lane { get; set; }
    public string? Role { get; set; }

    public int? Kills { get; set; }
    public int? Deaths { get; set; }
    public int? Assists { get; set; }

    public int? TotalMinionsKilled { get; set; }
    public int? NeutralMinionsKilled { get; set; }
    public int? GoldEarned { get; set; }
    public int? GoldSpent { get; set; }
    public int? TimePlayed { get; set; }

    public string? RiotIdGameName { get; set; }
    public string? RiotIdTagline { get; set; }
    public string? SummonerName { get; set; }
    public string? SummonerId { get; set; }
    public int? SummonerLevel { get; set; }

    [JsonExtensionData]
    public Dictionary<string, JsonElement>? ExtensionData { get; set; }

    public GameParticipant ToEntity() =>
        new (Puuid, ParticipantId, TeamId, Win, ChampionId, ChampionName, ChampLevel, IndividualPosition, TeamPosition, Lane, Role, Kills, Deaths, Assists, TotalMinionsKilled, NeutralMinionsKilled, GoldEarned,
            GoldSpent, TimePlayed, RiotIdGameName, RiotIdTagline, SummonerName, SummonerId, SummonerLevel);
}