using Microsoft.AspNetCore.Mvc;
using WeatherApp.Api.Models;
using WeatherApp.Api.Services;

namespace WeatherApp.Api.Controllers;

[ApiController]
[Route("api/weather")]
public class WeatherController : ControllerBase
{
    private readonly IWeatherService _weatherService;
    private readonly ILogger<WeatherController> _logger;

    public WeatherController(IWeatherService weatherService, ILogger<WeatherController> logger)
    {
        _weatherService = weatherService;
        _logger = logger;
    }

    [HttpGet("{cityName}")]
    public async Task<ActionResult<WeatherResponse>> GetWeather(string cityName)
    {
        try
        {
            var weather = await _weatherService.GetWeatherAsync(cityName);

            if (weather == null)
                return NotFound(new { message = $"No weather found for '{cityName}'." });

            return Ok(weather);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Weather failed for the {City}", cityName);
            return StatusCode(502, new { message = "Something went wrong with The weather service." });
        }
    }
}
