namespace BE.Riot.Console.Models;

public sealed class LobbyDto
{
    public LobbyMember[] Members { get; set; } = Array.Empty<LobbyMember>();
    public GameConfigDto? GameConfig { get; set; }
}

public sealed class LobbyMember 
{ 
    public string? SummonerName { get; set; } 
}

public sealed class GameConfigDto 
{ 
    public int? QueueId { get; set; } 
}

public sealed class ReadyCheckDto 
{ 
    public string? State { get; set; } 
}

public sealed class ChampSelectSession
{
    public TimerObj? Timer { get; set; }
    public int LocalPlayerCellId { get; set; }
    public List<List<ActionObj>> Actions { get; set; } = new();
    public List<TeamMember> MyTeam { get; set; } = new();
    public int? MySelectionChampionId { get; set; }
    public int? MyLockedChampionId { get; set; }
    public int? MyTeamIntentChampionId { get; set; }

    public sealed class TimerObj 
    { 
        public string? Phase { get; set; } 
    }
    
    public sealed class ActionObj 
    { 
        public int ActorCellId { get; set; }
        public bool IsInProgress { get; set; }
        public string? Type { get; set; }
        public int ChampionId { get; set; }
    }
    
    public sealed class TeamMember 
    {
        public int CellId { get; set; }
        public int ChampionId { get; set; }
        public int? ChampionPickIntent { get; set; }
        public int? Spell1Id { get; set; }
        public int? Spell2Id { get; set; }
    }
}

public sealed class LiveWrapper 
{ 
    public List<LiveEvent> Events { get; set; } = new(); 
}

public sealed class LiveEvent
{
    public int EventID { get; set; }
    public string EventName { get; set; } = "";
    public float EventTime { get; set; }
    public string? KillerName { get; set; }
    public string? VictimName { get; set; }
    public List<string>? Assisters { get; set; }
    public string? ItemName { get; set; }
    public string? Player { get; set; }
    public string? KillerTeam { get; set; }
    public string? DragonType { get; set; }
    public string? TurretKilled { get; set; }
    public string? InhibKilled { get; set; }
    public string? Result { get; set; }
}