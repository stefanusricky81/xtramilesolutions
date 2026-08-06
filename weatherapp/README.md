## Running

1. Open `WeatherApp.sln` in Visual Studio.
2. Press F5.
3. The browser opens on `http://localhost:5080`.

Or Test > Run All Tests in Visual Studio. The tests do not make any network calls.

## API

| Method | Route | Description |
| --- | --- | --- |
| GET | `/api/countries` | List of countries |
| GET | `/api/countries/{countryCode}/cities` | Cities in that country |
| GET | `/api/weather/{cityName}` | Current weather for that city |

## Using real OpenWeatherMap data

for the weather using openweathermap
To use the real API, get a free key from https://openweathermap.org/api and put it in
`appsettings.json`:

```json
"OpenWeatherMap": {
  "ApiKey": "your-key-here"
}
```

Restart the app. `Program.cs` picks `OpenWeatherMapService` when a key is present and
`MockWeatherService` when it is not.

New keys can take a couple of hours before OpenWeatherMap activates them.

## Notes on the implementation

**Fahrenheit.** OpenWeatherMap returns Kelvin by default, so the request has to include
`units=imperial` to get Fahrenheit. for convert to Celsius is in
`TemperatureConverter`, which is called from the service layer and covered by unit tests.

**Dew point.** The free OpenWeatherMap endpoint doesn't return a dew point, so it's
calculated from temperature and humidity using the Magnus formula.

**Visibility and pressure.** These come back in metres and hPa even when `units=imperial`
is set, so visibility is converted to miles in the service and pressure is left as hPa.

**Structure.**

```
src/WeatherApp.Api/
  Controllers/   CountriesController, WeatherController
  Models/        Country, City, WeatherResponse, OpenWeatherResponse
  Services/      ILocationService / LocationService
                 IWeatherService / OpenWeatherMapService / MockWeatherService
                 TemperatureConverter
  wwwroot/       index.html, site.css, site.js
tests/WeatherApp.Api.Tests/
```

Services and the HttpClient are registered in `Program.cs` and injected through
constructors. 

`OpenWeatherMapService` is registered with `AddHttpClient` so the HttpClient
is managed by `IHttpClientFactory`.

The front end is served from `wwwroot` by the same project


## Tests

| File | What it covers |
| --- | --- |
| `TemperatureConverterTests` | Fahrenheit to Celsius conversion, dew point calculation |
| `WeatherServiceTests` | Successful HTTP call and mapping, city not found, failed HTTP calls |
| `WeatherControllerTests` | Success, 404 and 502 responses, with a mocked service |
| `CountriesControllerTests` | Country and city lists, 404 for an unknown country |

`FakeHttpMessageHandler` returns a canned response to `HttpClient`, so the service tests
run without any network access.
