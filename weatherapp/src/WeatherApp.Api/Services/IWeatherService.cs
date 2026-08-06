using WeatherApp.Api.Models;

namespace WeatherApp.Api.Services;

public interface IWeatherService
{
    // Returns null if the city is not recognised.
    Task<WeatherResponse?> GetWeatherAsync(string cityName);
}
