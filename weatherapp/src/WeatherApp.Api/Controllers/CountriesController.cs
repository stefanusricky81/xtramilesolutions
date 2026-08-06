using Microsoft.AspNetCore.Mvc;
using WeatherApp.Api.Models;
using WeatherApp.Api.Services;

namespace WeatherApp.Api.Controllers;

[ApiController]
[Route("api/countries")]
public class CountriesController : ControllerBase
{
    private readonly ILocationService _locationService;

    public CountriesController(ILocationService locationService)
    {
        _locationService = locationService;
    }

    [HttpGet]
    public ActionResult<List<Country>> GetCountries()
    {
        return Ok(_locationService.GetCountries());
    }

    [HttpGet("{countryCode}/cities")]
    public ActionResult<List<City>> GetCities(string countryCode)
    {
        var cities = _locationService.GetCities(countryCode);

        if (cities == null)
            return NotFound(new { message = $"Country '{countryCode}' not found." });

        return Ok(cities);
    }
}
