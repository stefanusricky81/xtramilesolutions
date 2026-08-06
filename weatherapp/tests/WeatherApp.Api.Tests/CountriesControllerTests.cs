using Microsoft.AspNetCore.Mvc;
using Moq;
using WeatherApp.Api.Controllers;
using WeatherApp.Api.Models;
using WeatherApp.Api.Services;
using Xunit;

namespace WeatherApp.Api.Tests;

public class CountriesControllerTests
{
    private readonly Mock<ILocationService> _locationService = new();

    [Fact]
    public void GetCountrieslist()
    {
        var countries = new List<Country>
        {
            new() { Code = "ID", Name = "Indonesia" },
            new() { Code = "GB", Name = "United Kingdom" }
        };
        _locationService.Setup(s => s.GetCountries()).Returns(countries);

        var controller = new CountriesController(_locationService.Object);
        var result = controller.GetCountries();

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var value = Assert.IsType<List<Country>>(ok.Value);
        Assert.Equal(2, value.Count);
    }

    [Fact]
    public void GetCitiesList()
    {
        var cities = new List<City> { new() { Name = "Jakarta" } };
        _locationService.Setup(s => s.GetCities("ID")).Returns(cities);

        var controller = new CountriesController(_locationService.Object);
        var result = controller.GetCities("ID");

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Single(Assert.IsType<List<City>>(ok.Value));
    }

    [Fact]
    public void GetUnknowncountry()
    {
        _locationService.Setup(s => s.GetCities("ZZ")).Returns((List<City>?)null);

        var controller = new CountriesController(_locationService.Object);
        var result = controller.GetCities("ZZ");

        Assert.IsType<NotFoundObjectResult>(result.Result);
    }
}
