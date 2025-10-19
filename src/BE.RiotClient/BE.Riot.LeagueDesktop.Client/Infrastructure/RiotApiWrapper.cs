namespace BE.Riot.Console.Infrastructure;

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using BE.Riot.Console.Models;

public class RiotApiWrapper : IDisposable
{
    private readonly HttpClient _live;
    private readonly JsonSerializerOptions _json;
    private HttpClient? _lcu;
    private string? _lockfilePath;

    public RiotApiWrapper(string? lockfilePath = null)
    {
        _lockfilePath = lockfilePath;
        _live = new HttpClient { BaseAddress = new Uri("http://127.0.0.1:2999"), Timeout = TimeSpan.FromMilliseconds(1500) };
        _json = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public async Task EnsureConnection(CancellationToken ct)
    {
        if (_lcu != null) return;

        var path = LockfileHelper.ResolveLockfilePath(_lockfilePath);
        if (path == null) return;

        var li = LockfileHelper.ReadLockfile(path);
        if (li == null) return;

        var handler = new HttpClientHandler
        { ServerCertificateCustomValidationCallback = (_, __, ___, ____) => true };

        var http = new HttpClient(handler)
        {
            BaseAddress = new Uri($"{li.Protocol}://127.0.0.1:{li.Port}"),
            Timeout = TimeSpan.FromMilliseconds(1500)
        };
        var authBytes = Encoding.ASCII.GetBytes($"riot:{li.Password}");
        http.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authBytes));

        _lcu = http;
    }

    public void ResetConnection()
    {
        _lcu?.Dispose();
        _lcu = null;
    }

    public async Task<LobbyDto?> GetLobby(CancellationToken ct)
    {
        return await TryGet<LobbyDto>(_lcu!, "/lol-lobby/v2/lobby", ct);
    }

    public async Task<ReadyCheckDto?> GetReadyCheck(CancellationToken ct)
    {
        return await TryGet<ReadyCheckDto>(_lcu!, "/lol-matchmaking/v1/ready-check", ct);
    }

    public async Task<ChampSelectSession?> GetChampSelect(CancellationToken ct)
    {
        return await TryGet<ChampSelectSession>(_lcu!, "/lol-champ-select/v1/session", ct, notFoundIsNull: true);
    }

    public async Task<LiveWrapper?> GetLiveEvents(CancellationToken ct)
    {
        return await TryGet<LiveWrapper>(_live, "/liveclientdata/eventdata", ct);
    }

    private static async Task<T?> TryGet<T>(HttpClient http, string url, CancellationToken ct, bool notFoundIsNull = false)
    {
        try
        {
            using var res = await http.GetAsync(url, ct);
            if (notFoundIsNull && (int)res.StatusCode == 404) return default;
            if (!res.IsSuccessStatusCode) return default;
            var json = await res.Content.ReadAsStringAsync(ct);
            return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch { return default; }
    }

    public void Dispose()
    {
        _lcu?.Dispose();
        _live.Dispose();
    }
}