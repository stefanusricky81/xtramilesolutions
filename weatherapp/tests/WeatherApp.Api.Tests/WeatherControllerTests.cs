using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using WeatherApp.Api.Controllers;
using WeatherApp.Api.Models;
using WeatherApp.Api.Services;
using Xunit;

namespace WeatherApp.Api.Tests;

public class WeatherControllerTests
{
    private readonly Mock<IWeatherService> _weatherService = new();

    private WeatherController CreateController() =>
        new(_weatherService.Object, NullLogger<WeatherController>.Instance);

    [Fact]
    public async Task GetWeather()
    {
        var weather = new WeatherResponse { Location = "London, GB", TemperatureF = 53.6 };
        _weatherService.Setup(s => s.GetWeatherAsync("London")).ReturnsAsync(weather);

        var result = await CreateController().GetWeather("London");

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(weather, ok.Value);
    }

    [Fact]
    public async Task GetWeatherUnknownCities()
    {
        _weatherService.Setup(s => s.GetWeatherAsync(It.IsAny<string>()))
            .ReturnsAsync((WeatherResponse?)null);

        var result = await CreateController().GetWeather("Atlantis");

        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetServiceWeatherFails()
    {
        _weatherService.Setup(s => s.GetWeatherAsync(It.IsAny<string>()))
            .ThrowsAsync(new HttpRequestException("upstream is down"));

        var result = await CreateController().GetWeather("London");

        var status = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(502, status.StatusCode);
    }
}
