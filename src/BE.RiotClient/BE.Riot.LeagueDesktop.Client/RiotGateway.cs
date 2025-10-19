namespace BE.Riot.Console;

using System;
using System.Linq;
using System.Collections.Generic;
using BE.Riot.Console.Events;
using BE.Riot.Console.Models;
using BE.Riot.Console.Infrastructure;

public class RiotGateway
{
    private readonly RiotApiWrapper _api;

    // Snapshots für State-Tracking
    private LobbyDto? _lobbyPrev;
    private ReadyCheckDto? _rcPrev;
    private ChampSelectSession? _csPrev;
    private string? _csFingerprintPrev;
    private int _lastLiveEventId = -1;

    public RiotGateway(RiotApiWrapper api)
    {
        _api = api ?? throw new ArgumentNullException(nameof(api));
    }

    public IEnumerable<IInGameEvent> ProcessLobbyUpdate(LobbyDto? lobby)
    {
        if (_lobbyPrev == null && lobby != null)
        {
            yield return new LobbyCreatedEvent(lobby.GameConfig?.QueueId?.ToString() ?? "unknown",
                lobby.Members.Select(m => m.SummonerName ?? "?").ToList());
        }
        if (_lobbyPrev != null && lobby != null)
        {
            var oldMembers = _lobbyPrev.Members.Select(m => m.SummonerName ?? "?").OrderBy(x => x).ToList();
            var newMembers = lobby.Members.Select(m => m.SummonerName ?? "?").OrderBy(x => x).ToList();
            if (!oldMembers.SequenceEqual(newMembers))
                yield return new LobbyMembersChangedEvent(oldMembers, newMembers);

            var oldQueue = _lobbyPrev.GameConfig?.QueueId?.ToString() ?? "unknown";
            var newQueue = lobby.GameConfig?.QueueId?.ToString() ?? "unknown";
            if (!String.Equals(oldQueue, newQueue, StringComparison.OrdinalIgnoreCase))
                yield return new LobbyQueueChangedEvent(oldQueue, newQueue);
        }
        _lobbyPrev = lobby;
    }

    public IEnumerable<IInGameEvent> ProcessReadyCheck(ReadyCheckDto? rc)
    {
        if (_rcPrev == null && rc != null)
            yield return new ReadyCheckStartedEvent();

        if (_rcPrev != null && rc != null && !String.Equals(_rcPrev.State ?? "", rc.State ?? "", StringComparison.OrdinalIgnoreCase))
            yield return new ReadyCheckUpdatedEvent(rc.State ?? "Unknown");

        if (_rcPrev != null && rc == null)
            yield return new ReadyCheckFinishedEvent("Cancelled");

        if (rc != null && (ChampSelectHelper.EqualsIgnore(rc.State,"EveryoneReady") || ChampSelectHelper.EqualsIgnore(rc.State,"Failed") || ChampSelectHelper.EqualsIgnore(rc.State,"TimedOut")))
            yield return new ReadyCheckFinishedEvent(rc.State!);

        _rcPrev = rc;
    }

    public IEnumerable<IInGameEvent> ProcessChampSelect(ChampSelectSession? cs)
    {
        var fpNew = ChampSelectHelper.ComputeSessionFingerprint(cs);
        if (fpNew != _csFingerprintPrev)
        {
            if (_csPrev?.Timer?.Phase != cs?.Timer?.Phase)
                yield return new CsPhaseChangedEvent(_csPrev?.Timer?.Phase, cs?.Timer?.Phase);

            if (_csPrev?.MyTeamIntentChampionId != cs?.MyTeamIntentChampionId)
                yield return new CsMyIntentChangedEvent(_csPrev?.MyTeamIntentChampionId, cs?.MyTeamIntentChampionId);

            if (_csPrev?.MySelectionChampionId != cs?.MySelectionChampionId)
                yield return new CsMySelectionChangedEvent(_csPrev?.MySelectionChampionId, cs?.MySelectionChampionId);

            if (_csPrev?.MyLockedChampionId != cs?.MyLockedChampionId)
                yield return new CsMyLockChangedEvent(_csPrev?.MyLockedChampionId, cs?.MyLockedChampionId);

            foreach (var ev in DetectTeamChanges(cs))
                yield return ev;

            foreach (var ev in DetectBanChanges(cs))
                yield return ev;

            _csPrev = cs;
            _csFingerprintPrev = fpNew;
        }
    }

    private IEnumerable<IInGameEvent> DetectTeamChanges(ChampSelectSession? cs)
    {
        var oldLocked = _csPrev?.MyTeam?.ToDictionary(t => t.CellId, t => t.ChampionId) ?? new Dictionary<int,int>();
        var newLocked = cs?.MyTeam?.ToDictionary(t => t.CellId, t => t.ChampionId) ?? new Dictionary<int,int>();
        var cellSet = new HashSet<int>(oldLocked.Keys.Concat(newLocked.Keys));
        var changes = new List<(int,int,int)>();
        foreach (var cell in cellSet)
        {
            oldLocked.TryGetValue(cell, out var o);
            newLocked.TryGetValue(cell, out var n);
            if (o != n) changes.Add((cell, o, n));
        }
        if (changes.Count > 0)
            yield return new CsTeamLockedChangedEvent(changes);
    }

    private IEnumerable<IInGameEvent> DetectBanChanges(ChampSelectSession? cs)
    {
        var oldBans = ChampSelectHelper.ComputeBanSet(_csPrev);
        var newBans = ChampSelectHelper.ComputeBanSet(cs);
        if (!oldBans.SetEquals(newBans))
            yield return new CsBansChangedEvent(oldBans, newBans);
    }

    public IEnumerable<IInGameEvent> ProcessLiveEvents(LiveWrapper? wrapper)
    {
        if (wrapper?.Events == null) yield break;

        foreach (var ev in wrapper.Events.Where(e => e.EventID > _lastLiveEventId).OrderBy(e => e.EventID))
        {
            var mapped = MapLiveEvent(ev);
            if (mapped != null) yield return mapped;
            _lastLiveEventId = ev.EventID;
        }
    }

    private static IInGameEvent MapLiveEvent(LiveEvent e)
    {
        return e.EventName switch
        {
            "GameStart"     => new GameStartedEvent(e.EventTime),
            "GameEnd"       => new GameEndedEvent(e.EventTime, e.Result ?? "Unknown"),
            "ChampionKill"  => !string.IsNullOrWhiteSpace(e.KillerName) && !string.IsNullOrWhiteSpace(e.VictimName)
                                ? new ChampionKillEvent(e.EventTime, e.KillerName!, e.VictimName!, e.Assisters ?? new())
                                : null,
            "ItemPurchased" => !string.IsNullOrWhiteSpace(e.ItemName) && !string.IsNullOrWhiteSpace(e.Player)
                                ? new ItemPurchasedEvent(e.EventTime, e.Player!, e.ItemName!)
                                : null,
            "ItemSold"      => !string.IsNullOrWhiteSpace(e.ItemName) && !string.IsNullOrWhiteSpace(e.Player)
                                ? new ItemSoldEvent(e.EventTime, e.Player!, e.ItemName!)
                                : null,
            "DragonKill"    => new DragonKillEvent(e.EventTime, e.DragonType ?? "UNKNOWN", e.KillerTeam ?? "UNKNOWN"),
            "BaronKill"     => new BaronKillEvent(e.EventTime, e.KillerTeam ?? "UNKNOWN"),
            "HeraldKill"    => new HeraldKillEvent(e.EventTime, e.KillerTeam ?? "UNKNOWN"),
            "TurretKilled"  => new TurretKilledEvent(e.EventTime, e.TurretKilled ?? "TURRET", e.KillerTeam ?? "UNKNOWN"),
            "InhibKilled"   => new InhibitorKilledEvent(e.EventTime, e.InhibKilled ?? "INHIB", e.KillerTeam ?? "UNKNOWN"),
            _ => new GenericEvent(e.EventName ?? "Unknown", e.EventTime, 
                    new Dictionary<string, string>(e.GetType()
                        .GetProperties()
                        .Where(p => p.GetValue(e) != null)
                        .Select(p => new KeyValuePair<string, string>(
                            p.Name, 
                            p.GetValue(e)?.ToString() ?? string.Empty
                        )))
                )
        };
    }
}