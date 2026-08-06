using System.Text.Json.Serialization;

namespace WeatherApp.Api.Models;

public class OpenWeatherResponse
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("dt")]
    public long Dt { get; set; }

    [JsonPropertyName("visibility")]
    public int Visibility { get; set; }

    [JsonPropertyName("weather")]
    public List<WeatherCondition> Weather { get; set; } = new();

    [JsonPropertyName("main")]
    public MainBlock? Main { get; set; }

    [JsonPropertyName("wind")]
    public WindBlock? Wind { get; set; }

    [JsonPropertyName("sys")]
    public SysBlock? Sys { get; set; }
}

public class WeatherCondition
{
    [JsonPropertyName("description")]
    public string Description { get; set; } = "";
}

public class MainBlock
{
    [JsonPropertyName("temp")]
    public double Temp { get; set; }

    [JsonPropertyName("pressure")]
    public double Pressure { get; set; }

    [JsonPropertyName("humidity")]
    public int Humidity { get; set; }
}

public class WindBlock
{
    [JsonPropertyName("speed")]
    public double Speed { get; set; }

    [JsonPropertyName("deg")]
    public int Deg { get; set; }
}

public class SysBlock
{
    [JsonPropertyName("country")]
    public string Country { get; set; } = "";
}
