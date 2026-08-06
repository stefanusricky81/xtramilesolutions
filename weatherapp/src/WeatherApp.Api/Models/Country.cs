namespace WeatherApp.Api.Models;

public class Country
{
    public string Code { get; set; } = "";
    public string Name { get; set; } = "";
    public List<City> Cities { get; set; } = new();
}

public class City
{
    public string Name { get; set; } = "";
}
