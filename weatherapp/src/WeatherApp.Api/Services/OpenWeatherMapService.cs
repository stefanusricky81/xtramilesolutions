using WeatherApp.Api.Models;

namespace WeatherApp.Api.Services;

public class OpenWeatherMapService : IWeatherService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public OpenWeatherMapService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["OpenWeatherMap:ApiKey"] ?? "";
    }

    public async Task<WeatherResponse?> GetWeatherAsync(string cityName)
    {
        // units=imperial is important: without it OpenWeatherMap returns Kelvin,
        // and the Fahrenheit to Celsius conversion below would be wrong.
        var url = $"data/2.5/weather?q={Uri.EscapeDataString(cityName)}&units=imperial&appid={_apiKey}";

        var response = await _httpClient.GetAsync(url);

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return null;

        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException($"OpenWeatherMap returned {(int)response.StatusCode}.");

        var data = await response.Content.ReadFromJsonAsync<OpenWeatherResponse>();

        if (data?.Main == null)
            throw new HttpRequestException("Failed to load OpenWeatherMap.");

        return Map(data);
    }

    private static WeatherResponse Map(OpenWeatherResponse data)
    {
        var temperatureF = data.Main!.Temp;
        var humidity = data.Main.Humidity;
        var dewPointF = TemperatureConverter.DewPointFahrenheit(temperatureF, humidity);

        return new WeatherResponse
        {
            Location = $"{data.Name}, {data.Sys?.Country}",
            TimeUtc = DateTimeOffset.FromUnixTimeSeconds(data.Dt).UtcDateTime,
            WindSpeedMph = Math.Round(data.Wind?.Speed ?? 0, 1),
            WindDirection = DegreesToCompass(data.Wind?.Deg ?? 0),

            // Visibility comes back in metres even with units=imperial.
            VisibilityMiles = Math.Round(data.Visibility / 1609.344, 1),

            SkyConditions = data.Weather.FirstOrDefault()?.Description ?? "unknown",
            TemperatureF = Math.Round(temperatureF, 1),
            TemperatureC = Math.Round(TemperatureConverter.FahrenheitToCelsius(temperatureF), 1),
            DewPointF = Math.Round(dewPointF, 1),
            DewPointC = Math.Round(TemperatureConverter.FahrenheitToCelsius(dewPointF), 1),
            RelativeHumidity = humidity,
            PressureHpa = data.Main.Pressure
        };
    }

    private static readonly string[] Compass =
    {
        "N", "NNE", "NE", "ENE", "E", "ESE", "SE", "SSE",
        "S", "SSW", "SW", "WSW", "W", "WNW", "NW", "NNW"
    };

    private static string DegreesToCompass(int degrees)
    {
        var normalised = ((degrees % 360) + 360) % 360;
        return Compass[(int)Math.Round(normalised / 22.5) % 16];
    }
}
