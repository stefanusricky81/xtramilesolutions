using WeatherApp.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<ILocationService, LocationService>();

// Use the real API when a key is configured, otherwise fall back to mock data
// so the app still runs without any setup.
var apiKey = builder.Configuration["OpenWeatherMap:ApiKey"];

if (string.IsNullOrWhiteSpace(apiKey))
{
    builder.Services.AddScoped<IWeatherService, MockWeatherService>();
}
else
{
    builder.Services.AddHttpClient<IWeatherService, OpenWeatherMapService>(client =>
    {
        client.BaseAddress = new Uri("https://api.openweathermap.org/");
        client.Timeout = TimeSpan.FromSeconds(10);
    });
}

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();
app.MapControllers();

app.Run();
