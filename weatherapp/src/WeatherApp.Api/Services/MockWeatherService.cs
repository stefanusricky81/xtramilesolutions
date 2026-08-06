using WeatherApp.Api.Models;

namespace WeatherApp.Api.Services;

// Used when no OpenWeatherMap API key is configured, so the app still runs.
public class MockWeatherService : IWeatherService
{
    private readonly ILocationService _locationService;

    public MockWeatherService(ILocationService locationService)
    {
        _locationService = locationService;
    }

    public Task<WeatherResponse?> GetWeatherAsync(string cityName)
    {
        var country = _locationService.GetCountries()
            .FirstOrDefault(c => c.Cities.Any(city =>
                city.Name.Equals(cityName, StringComparison.OrdinalIgnoreCase)));

        if (country == null)
            return Task.FromResult<WeatherResponse?>(null);

        // Fixed sample values so the demo is predictable.
        const double temperatureF = 68.5;
        const int humidity = 65;
        var dewPointF = TemperatureConverter.DewPointFahrenheit(temperatureF, humidity);

        var result = new WeatherResponse
        {
            Location = $"{cityName}, {country.Code}",
            TimeUtc = DateTime.UtcNow,
            WindSpeedMph = 8.5,
            WindDirection = "WSW",
            VisibilityMiles = 6.2,
            SkyConditions = "broken clouds",
            TemperatureF = temperatureF,
            TemperatureC = Math.Round(TemperatureConverter.FahrenheitToCelsius(temperatureF), 1),
            DewPointF = Math.Round(dewPointF, 1),
            DewPointC = Math.Round(TemperatureConverter.FahrenheitToCelsius(dewPointF), 1),
            RelativeHumidity = humidity,
            PressureHpa = 1013
        };

        return Task.FromResult<WeatherResponse?>(result);
    }
}
