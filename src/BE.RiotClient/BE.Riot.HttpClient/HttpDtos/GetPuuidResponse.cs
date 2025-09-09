using System.Text.Json.Serialization;

namespace BE.Riot.Http.HttpDtos;

internal sealed class GetPuuidResponse
{
    [JsonPropertyName("puuid")]
    public string Puuid { get; init; }
    
    [JsonPropertyName("gameName")]
    public string GameName { get; init; }

    [JsonPropertyName("tagLine")]
    public string TagLine { get; init; }
}