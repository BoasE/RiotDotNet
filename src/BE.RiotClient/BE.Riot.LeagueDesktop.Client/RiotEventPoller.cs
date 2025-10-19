using BE.Riot.Console.Infrastructure;

namespace BE.Riot.Console;

using System;
using System.Threading;
using System.Threading.Tasks;
using BE.Riot.Console.Events;

public class RiotEventPoller : IDisposable
{
    public event EventHandler<IInGameEvent>? InGameEvent;
    public bool IsRunning => _loop != null && !_loop.IsCompleted;

    private readonly RiotGateway _gateway;
    private readonly RiotApiWrapper _api;
    private CancellationTokenSource? _cts;
    private Task? _loop;
    private readonly int _pregamePollMs;
    private readonly int _ingamePollMs;

    public RiotEventPoller(string? lockfilePath = null, int pregamePollMs = 800, int ingamePollMs = 500)
    {
        _api = new RiotApiWrapper(lockfilePath);
        _gateway = new RiotGateway(_api);
        _pregamePollMs = pregamePollMs;
        _ingamePollMs = ingamePollMs;
    }

    public void Start()
    {
        if (IsRunning) return;
        _cts = new CancellationTokenSource();
        _loop = Task.Run(() => PollLoopAsync(_cts.Token));
    }

    public void Stop()
    {
        if (!IsRunning) return;
        _cts!.Cancel();
        try { _loop!.Wait(800); } catch { }
        _loop = null;
    }

    private async Task PollLoopAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            try
            {
                await _api.EnsureConnection(ct);
                await PollAllAsync(ct);
            }
            catch
            {
                _api.ResetConnection();
            }

            try { await Task.Delay(100, ct); } catch (OperationCanceledException) { }
        }
    }

    private async Task PollAllAsync(CancellationToken ct)
    {
        // Poll pre-game state
        var lobby = await _api.GetLobby(ct);
        foreach (var ev in _gateway.ProcessLobbyUpdate(lobby))
            OnInGameEvent(ev);

        var rc = await _api.GetReadyCheck(ct);
        foreach (var ev in _gateway.ProcessReadyCheck(rc))
            OnInGameEvent(ev);

        var cs = await _api.GetChampSelect(ct);
        foreach (var ev in _gateway.ProcessChampSelect(cs))
            OnInGameEvent(ev);

        try { await Task.Delay(_pregamePollMs, ct); } catch (OperationCanceledException) { }

        // Poll in-game state
        var liveEvents = await _api.GetLiveEvents(ct);
        foreach (var ev in _gateway.ProcessLiveEvents(liveEvents))
            OnInGameEvent(ev);

        try { await Task.Delay(_ingamePollMs, ct); } catch (OperationCanceledException) { }
    }

    protected virtual void OnInGameEvent(IInGameEvent e) => InGameEvent?.Invoke(this, e);

    public void Dispose()
    {
        Stop();
        _api.Dispose();
        _cts?.Dispose();
    }
}