namespace ConsoleApp1;

public class WeatherResponse
{
    
    public double latitude { get; set; }
    public double longitude { get; set; }
    public CurrentWeatherUnits current_weather_units { get; set; }
    public CurrentWeather current_weather { get; set; }
}


public class CurrentWeather
{
    public string time { get; set; }
    public int interval { get; set; }
    public double temperature { get; set; }
    public double windspeed { get; set; }
    public int winddirection { get; set; }
    public int is_day { get; set; }
    public int weathercode { get; set; }
}


public class CurrentWeatherUnits
{
    public string time { get; set; }
    public string interval { get; set; }
    public string temperature { get; set; }
    public string windspeed { get; set; }
    public string winddirection { get; set; }
    public string is_day { get; set; }
    public string weathercode { get; set; }
}