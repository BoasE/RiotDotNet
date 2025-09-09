using System.Text.Json;
using BE.Riot.Http.HttpDtos;

namespace BE.Riot.Http;

public sealed class RiotHttpClient : IRiotClient
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _host;

    public RiotHttpClient(string host, string apiKey)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(host);

        host = host.Trim();
        if (!host.EndsWith("/"))
        {
            host += "/";
        }

        _host = host;
        _apiKey = apiKey;
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("X-Riot-Token", _apiKey);
    }

    /// <see cref="https://developer.riotgames.com/apis#match-v5/GET_getMatchIdsByPUUID"/>
    public async Task<ISet<MatchId>> GetLatestMatchIdsByPuuId(string puuId)
    {
        var url = RiotPathBuilder.MatchesIdByPuuid(_host, puuId);

        var result = await _httpClient.GetAsync(url);

        var content = await result.Content.ReadAsStringAsync();
        var items = JsonSerializer.Deserialize<List<string>>(content);

        return items?.Select(x => new MatchId(x)).ToHashSet()
               ?? [];
    }

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