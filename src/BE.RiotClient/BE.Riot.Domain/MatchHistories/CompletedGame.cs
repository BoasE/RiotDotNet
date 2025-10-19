namespace BE.Riot.MatchHistories;

public sealed class GameParticipant
{
    public string? Puuid { get; }
    public int? ParticipantId { get; }
    public int? TeamId { get; }
    public bool? Win { get; }

    public int? ChampionId { get; }
    public string? ChampionName { get; }
    public int? ChampLevel { get; }

    public string? IndividualPosition { get; }
    public string? TeamPosition { get; }
    public string? Lane { get; }
    public string? Role { get; }

    public int? Kills { get; }
    public int? Deaths { get; }
    public int? Assists { get; }

    public int? TotalMinionsKilled { get; }
    public int? NeutralMinionsKilled { get; }
    public int? GoldEarned { get; }
    public int? GoldSpent { get; }
    public int? TimePlayed { get; }

    public string? RiotIdGameName { get; }
    public string? RiotIdTagline { get; }
    public string? SummonerName { get; }
    public string? SummonerId { get; }
    public int? SummonerLevel { get; }

    public GameParticipant(string? puuid, int? participantId, int? teamId, bool? win, int? championId, string? championName, int? champLevel, string? individualPosition, string? teamPosition, string? lane, string? role, int? kills, int? deaths, int? assists, int? totalMinionsKilled, int? neutralMinionsKilled, int? goldEarned, int? goldSpent, int? timePlayed, string? riotIdGameName, string? riotIdTagline, string? summonerName, string? summonerId, int? summonerLevel)
    {
        Puuid = puuid;
        ParticipantId = participantId;
        TeamId = teamId;
        Win = win;
        ChampionId = championId;
        ChampionName = championName;
        ChampLevel = champLevel;
        IndividualPosition = individualPosition;
        TeamPosition = teamPosition;
        Lane = lane;
        Role = role;
        Kills = kills;
        Deaths = deaths;
        Assists = assists;
        TotalMinionsKilled = totalMinionsKilled;
        NeutralMinionsKilled = neutralMinionsKilled;
        GoldEarned = goldEarned;
        GoldSpent = goldSpent;
        TimePlayed = timePlayed;
        RiotIdGameName = riotIdGameName;
        RiotIdTagline = riotIdTagline;
        SummonerName = summonerName;
        SummonerId = summonerId;
        SummonerLevel = summonerLevel;
    }
}

public sealed class CompletedGame
{
    public string Id { get; }

    public string? EndOfGameResult { get; }
    public long? GameCreation { get; }
    public TimeSpan? GameDuration { get; }
    public DateTimeOffset? GameEndTimestamp { get; }
    public long? GameId { get; }
    public string? GameMode { get; }
    public string? GameName { get; }
    public DateTimeOffset? GameStartTimestamp { get; }
    public string? GameType { get; }
    public string? GameVersion { get; }
    public int? MapId { get; }

    public List<GameParticipant> Participants { get; }

    public CompletedGame(string id, IEnumerable<GameParticipant> participants, string? endOfGameResult, long? gameCreation, int? gameDuration, long? gameEndTimestamp, long? gameId, string? gameMode, string? gameName, long? gameStartTimestamp, string? gameType, string? gameVersion, int? mapId)
    {
        Id = id;
        EndOfGameResult = endOfGameResult;
        GameCreation = gameCreation;
        GameDuration = gameDuration != null ? TimeSpan.FromSeconds(gameDuration.Value) : null;
        GameEndTimestamp = gameEndTimestamp != null ? DateTimeOffset.FromUnixTimeMilliseconds(gameEndTimestamp.Value) : null;
        GameId = gameId;
        GameMode = gameMode;
        GameName = gameName;
        GameStartTimestamp = gameStartTimestamp != null ? DateTimeOffset.FromUnixTimeMilliseconds(gameStartTimestamp.Value) : null;
        GameType = gameType;
        GameVersion = gameVersion;
        MapId = mapId;
        Participants = participants.ToList();
    }
}