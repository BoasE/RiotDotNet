namespace BE.Riot.Console.Events;

public sealed class LobbyCreatedEvent : InGameEventBase
{
    public string QueueId { get; }
    public IReadOnlyList<string> Members { get; }
    public LobbyCreatedEvent(string queueId, IReadOnlyList<string> members) : base(nameof(LobbyCreatedEvent))
    { QueueId = queueId; Members = members; }
    public override string ToString() => $"[{Stamp()}] [Lobby] created, queue={QueueId}, members=[{string.Join(", ", Members)}]";
}

public sealed class LobbyMembersChangedEvent : InGameEventBase
{
    public IReadOnlyList<string> OldMembers { get; }
    public IReadOnlyList<string> NewMembers { get; }
    public LobbyMembersChangedEvent(IReadOnlyList<string> oldM, IReadOnlyList<string> newM) : base(nameof(LobbyMembersChangedEvent))
    { OldMembers = oldM; NewMembers = newM; }
    public override string ToString() => $"[{Stamp()}] [Lobby] members: [{string.Join(", ", OldMembers)}] -> [{string.Join(", ", NewMembers)}]";
}

public sealed class LobbyQueueChangedEvent : InGameEventBase
{
    public string OldQueueId { get; }
    public string NewQueueId { get; }
    public LobbyQueueChangedEvent(string oldQ, string newQ) : base(nameof(LobbyQueueChangedEvent))
    { OldQueueId = oldQ; NewQueueId = newQ; }
    public override string ToString() => $"[{Stamp()}] [Lobby] queue: {OldQueueId} -> {NewQueueId}";
}

public sealed class ReadyCheckStartedEvent : InGameEventBase
{
    public ReadyCheckStartedEvent() : base(nameof(ReadyCheckStartedEvent)) { }
    public override string ToString() => $"[{Stamp()}] [RC] started";
}

public sealed class ReadyCheckUpdatedEvent : InGameEventBase
{
    public string State { get; }
    public ReadyCheckUpdatedEvent(string state) : base(nameof(ReadyCheckUpdatedEvent)) { State = state; }
    public override string ToString() => $"[{Stamp()}] [RC] state: {State}";
}

public sealed class ReadyCheckFinishedEvent : InGameEventBase
{
    public string Result { get; }
    public ReadyCheckFinishedEvent(string result) : base(nameof(ReadyCheckFinishedEvent)) { Result = result; }
    public override string ToString() => $"[{Stamp()}] [RC] finished: {Result}";
}

public sealed class CsPhaseChangedEvent : InGameEventBase
{
    public string? OldPhase { get; }
    public string? NewPhase { get; }
    public CsPhaseChangedEvent(string? oldP, string? newP) : base(nameof(CsPhaseChangedEvent))
    { OldPhase = oldP; NewPhase = newP; }
    public override string ToString() => $"[{Stamp()}] [CS] phase: {OldPhase ?? "-"} -> {NewPhase ?? "-"}";
}

public sealed class CsMyIntentChangedEvent : InGameEventBase
{
    public int? OldChampionId { get; }
    public int? NewChampionId { get; }
    public CsMyIntentChangedEvent(int? oldId, int? newId) : base(nameof(CsMyIntentChangedEvent))
    { OldChampionId = oldId; NewChampionId = newId; }
    public override string ToString() => $"[{Stamp()}] [CS] my intent: {OldChampionId?.ToString() ?? "-"} -> {NewChampionId?.ToString() ?? "-"}";
}

public sealed class CsMySelectionChangedEvent : InGameEventBase
{
    public int? OldChampionId { get; }
    public int? NewChampionId { get; }
    public CsMySelectionChangedEvent(int? oldId, int? newId) : base(nameof(CsMySelectionChangedEvent))
    { OldChampionId = oldId; NewChampionId = newId; }
    public override string ToString() => $"[{Stamp()}] [CS] my selection: {OldChampionId?.ToString() ?? "-"} -> {NewChampionId?.ToString() ?? "-"}";
}

public sealed class CsMyLockChangedEvent : InGameEventBase
{
    public int? OldChampionId { get; }
    public int? NewChampionId { get; }
    public CsMyLockChangedEvent(int? oldId, int? newId) : base(nameof(CsMyLockChangedEvent))
    { OldChampionId = oldId; NewChampionId = newId; }
    public override string ToString() => $"[{Stamp()}] [CS] my lock: {OldChampionId?.ToString() ?? "-"} -> {NewChampionId?.ToString() ?? "-"}";
}

public sealed class CsTeamLockedChangedEvent : InGameEventBase
{
    public IReadOnlyList<(int CellId, int OldChampId, int NewChampId)> Changes { get; }
    public CsTeamLockedChangedEvent(IReadOnlyList<(int, int, int)> changes) : base(nameof(CsTeamLockedChangedEvent))
    { Changes = changes; }
    public override string ToString()
    {
        var parts = new List<string>(Changes.Count);
        foreach (var (cell, oldC, newC) in Changes) parts.Add($"cell {cell}: {oldC} -> {newC}");
        return $"[{Stamp()}] [CS] team locked: {string.Join(" | ", parts)}";
    }
}

public sealed class CsBansChangedEvent : InGameEventBase
{
    public IReadOnlyCollection<int> OldBans { get; }
    public IReadOnlyCollection<int> NewBans { get; }
    public CsBansChangedEvent(IReadOnlyCollection<int> oldB, IReadOnlyCollection<int> newB) : base(nameof(CsBansChangedEvent))
    { OldBans = oldB; NewBans = newB; }
    public override string ToString() => $"[{Stamp()}] [CS] bans: [{string.Join(",", OldBans)}] -> [{string.Join(",", NewBans)}]";
}