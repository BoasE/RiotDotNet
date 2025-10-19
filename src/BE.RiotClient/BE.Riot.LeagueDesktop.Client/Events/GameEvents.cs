namespace BE.Riot.Console.Events;

public class GameStartedEvent : GameEventBase
{
    public override string Name => "GameStarted";
    public GameStartedEvent(float eventTime) : base(eventTime) { }
}

public class GameEndedEvent : GameEventBase
{
    public override string Name => "GameEnded";
    public string Result { get; }
    public GameEndedEvent(float eventTime, string result) : base(eventTime) 
    {
        Result = result;
    }
}

public class ChampionKillEvent : GameEventBase
{
    public override string Name => "ChampionKill";
    public string KillerName { get; }
    public string VictimName { get; }
    public List<string> Assisters { get; }

    public ChampionKillEvent(float eventTime, string killerName, string victimName, List<string> assisters) : base(eventTime)
    {
        KillerName = killerName;
        VictimName = victimName;
        Assisters = assisters;
    }
}

public class ItemPurchasedEvent : GameEventBase
{
    public override string Name => "ItemPurchased";
    public string Player { get; }
    public string ItemName { get; }

    public ItemPurchasedEvent(float eventTime, string player, string itemName) : base(eventTime)
    {
        Player = player;
        ItemName = itemName;
    }
}

public class ItemSoldEvent : GameEventBase
{
    public override string Name => "ItemSold";
    public string Player { get; }
    public string ItemName { get; }

    public ItemSoldEvent(float eventTime, string player, string itemName) : base(eventTime)
    {
        Player = player;
        ItemName = itemName;
    }
}

public class DragonKillEvent : GameEventBase
{
    public override string Name => "DragonKill";
    public string DragonType { get; }
    public string KillerTeam { get; }

    public DragonKillEvent(float eventTime, string dragonType, string killerTeam) : base(eventTime)
    {
        DragonType = dragonType;
        KillerTeam = killerTeam;
    }
}

public class BaronKillEvent : GameEventBase
{
    public override string Name => "BaronKill";
    public string KillerTeam { get; }

    public BaronKillEvent(float eventTime, string killerTeam) : base(eventTime)
    {
        KillerTeam = killerTeam;
    }
}

public class HeraldKillEvent : GameEventBase
{
    public override string Name => "HeraldKill";
    public string KillerTeam { get; }

    public HeraldKillEvent(float eventTime, string killerTeam) : base(eventTime)
    {
        KillerTeam = killerTeam;
    }
}

public class TurretKilledEvent : GameEventBase
{
    public override string Name => "TurretKilled";
    public string TurretName { get; }
    public string KillerTeam { get; }

    public TurretKilledEvent(float eventTime, string turretName, string killerTeam) : base(eventTime)
    {
        TurretName = turretName;
        KillerTeam = killerTeam;
    }
}

public class InhibitorKilledEvent : GameEventBase
{
    public override string Name => "InhibitorKilled";
    public string InhibName { get; }
    public string KillerTeam { get; }

    public InhibitorKilledEvent(float eventTime, string inhibName, string killerTeam) : base(eventTime)
    {
        InhibName = inhibName;
        KillerTeam = killerTeam;
    }
}

public class GenericEvent : GameEventBase
{
    public override string Name { get; }
    public Dictionary<string, string> Properties { get; }

    public GenericEvent(string name, float eventTime, Dictionary<string, string> properties) : base(eventTime)
    {
        Name = name;
        Properties = properties;
    }
}