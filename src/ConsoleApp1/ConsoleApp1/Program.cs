// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using ConsoleApp1;

Console.WriteLine("Hello, World!");





string url = "https://api.open-meteo.com/v1/forecast" +
             "?latitude=49.0069" +
             "&longitude=8.4037" +
             "&current_weather=true";
             
using HttpClient client = new HttpClient();
HttpResponseMessage response = await client.GetAsync(url);
response.EnsureSuccessStatusCode(); // Fehler werfen bei z.B. 404

string content = await response.Content.ReadAsStringAsync();
var result = JsonSerializer.Deserialize<WeatherResponse>(content,new JsonSerializerOptions()
{
    
});


Console.WriteLine("Aktuelle Wetterdaten für Karlsruhe:");
Console.WriteLine(result);