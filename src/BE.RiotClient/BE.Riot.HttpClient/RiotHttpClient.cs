using System.Net;
using System.Text.Json;
using BE.Riot.Http.HttpDtos;
using BE.Riot.MatchHistories;

namespace BE.Riot.Http;

public sealed class RiotHttpClient : IRiotClient
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _host;
    private readonly ICompletedGameDetailCache _cache;

    public RiotHttpClient(string host, string apiKey, ICompletedGameDetailCache cache)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(host);

        host = host.Trim();
        if (!host.EndsWith("/"))
        {
            host += "/";
        }

        _cache = cache;
        _host = host;
        _apiKey = apiKey;
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("X-Riot-Token", _apiKey);
    }

    /// <see href="https://developer.riotgames.com/apis#match-v5/GET_getMatchIdsByPUUID"/>
    public async Task<ISet<MatchId>> GetLatestMatchIdsByPuuId(
        string puuId,
        int? count = null,
        int? start = null,
        DateTimeOffset? startTime = null,
        DateTimeOffset? endTime = null,
        int? queue = null,
        string? type = null)
    {
        var url = RiotPathBuilder.MatchesIdByPuuid(
            _host,
            puuId,
            count: count,
            start: start,
            startTime: startTime,
            endTime: endTime,
            queue: queue,
            type: type);

        var result = await _httpClient.GetAsync(url);

        var content = await result.Content.ReadAsStringAsync();
        var items = JsonSerializer.Deserialize<List<string>>(content);

        return items?.Select(x => new MatchId(x)).ToHashSet()
               ?? [];
    }

    public async Task<CompletedGame?> GetMatchById(string matchId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(matchId);

        bool fresh = false;
        string content = await _cache.GetCompletedGame(matchId);

        if (string.IsNullOrWhiteSpace(content))
        {
            fresh = true;

            var url = RiotPathBuilder.MatchById(_host, matchId);
            var response = await _httpClient.GetAsync(url);

            if (response.StatusCode == HttpStatusCode.NoContent ||
                response.StatusCode == HttpStatusCode.NotFound || response.StatusCode == HttpStatusCode.TooManyRequests)
            {
                return null;
            }

            content = await response.Content.ReadAsStringAsync();

            await Task.Delay(1000);
            await _cache.SetCompletedGameData(matchId, content);
        }

        var matchData = JsonSerializer.Deserialize<MatchResponse>(content, opt);

        // Analog zu den anderen Methoden: einfache Deserialisierung ohne zusätzliche Fehlerbehandlung
        var entity = matchData?.ToEntity();

        return entity;
    }

    private static readonly JsonSerializerOptions opt = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true,
    };
    public async Task<GetPuuIdResult> GetPuuIdBy(string summonerName, string tagName)
    {
        string url = RiotPathBuilder.PuiidBySummonerName(_host, summonerName, tagName);

        var result = await _httpClient.GetAsync(url);

        if (!result.IsSuccessStatusCode)
        {
            return GetPuuIdResult.None;
        }

        var content = await result.Content.ReadAsStringAsync();

        var dto = JsonSerializer.Deserialize<GetPuuidResponse>(content);
        if (dto == null)
        {
            return GetPuuIdResult.None;
        }

        return new GetPuuIdResult(true, new PuuId(dto.Puuid));
    }
}