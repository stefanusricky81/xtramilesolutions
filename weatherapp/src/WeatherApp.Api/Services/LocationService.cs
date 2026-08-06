using WeatherApp.Api.Models;

namespace WeatherApp.Api.Services;

// Seeded in memory, as allowed by the spec. No database.
public class LocationService : ILocationService
{
    private readonly List<Country> _countries = new()
    {
        new Country { Code = "ID", Name = "Indonesia", Cities = Cities("Jakarta", "Surabaya", "Bandung", "Medan", "Denpasar") },
        new Country { Code = "US", Name = "United States", Cities = Cities("New York", "Los Angeles", "Chicago", "Seattle") },
        new Country { Code = "GB", Name = "United Kingdom", Cities = Cities("London", "Manchester", "Edinburgh") },
        new Country { Code = "JP", Name = "Japan", Cities = Cities("Tokyo", "Osaka", "Sapporo") },
        new Country { Code = "AU", Name = "Australia", Cities = Cities("Sydney", "Melbourne", "Perth") },
        new Country { Code = "SG", Name = "Singapore", Cities = Cities("Singapore") }
    };

    public List<Country> GetCountries() => _countries;

    public List<City>? GetCities(string countryCode)
    {
        var country = _countries.FirstOrDefault(c =>
            c.Code.Equals(countryCode, StringComparison.OrdinalIgnoreCase));

        return country?.Cities;
    }

    private static List<City> Cities(params string[] names) =>
        names.Select(n => new City { Name = n }).ToList();
}
