using WeatherApp.Api.Models;

namespace WeatherApp.Api.Services;

public interface ILocationService
{
    List<Country> GetCountries();
    List<City>? GetCities(string countryCode);
}
