using System.Net;
using Microsoft.Extensions.Configuration;
using WeatherApp.Api.Services;
using Xunit;

namespace WeatherApp.Api.Tests;

public class WeatherServiceTests
{
    private const string SampleJson = """
    {
      "weather": [ { "description": "broken clouds" } ],
      "main": { "temp": 53.6, "pressure": 1017, "humidity": 72 },
      "visibility": 10000,
      "wind": { "speed": 8.05, "deg": 240 },
      "dt": 1773478800,
      "sys": { "country": "GB" },
      "name": "London"
    }
    """;

    private static OpenWeatherMapService CreateService(HttpStatusCode statusCode, string json)
    {
        var httpClient = new HttpClient(new FakeHttpMessageHandler(statusCode, json))
        {
            BaseAddress = new Uri("https://api.openweathermap.org/")
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["OpenWeatherMap:ApiKey"] = "test-key"
            })
            .Build();

        return new OpenWeatherMapService(httpClient, configuration);
    }

    [Fact]
    public async Task COnvertFahreinttoCelsius()
    {
        var service = CreateService(HttpStatusCode.OK, SampleJson);

        var result = await service.GetWeatherAsync("London");

        Assert.NotNull(result);
        Assert.Equal("London, GB", result.Location);
        Assert.Equal(53.6, result.TemperatureF);
        Assert.Equal(12.0, result.TemperatureC);
        Assert.Equal(6.2, result.VisibilityMiles);
        Assert.Equal("broken clouds", result.SkyConditions);
    }

    [Fact]
    public async Task CityNotFound()
    {
        var service = CreateService(HttpStatusCode.NotFound, "{}");

        var result = await service.GetWeatherAsync("Atlantis");

        Assert.Null(result);
    }

    [Theory]
    [InlineData(HttpStatusCode.Unauthorized)]
    [InlineData(HttpStatusCode.InternalServerError)]
    public async Task GetFails(HttpStatusCode statusCode)
    {
        var service = CreateService(statusCode, "{}");

        await Assert.ThrowsAsync<HttpRequestException>(() => service.GetWeatherAsync("London"));
    }
}
