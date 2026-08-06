namespace WeatherApp.Api.Services;

public static class TemperatureConverter
{
    public static double FahrenheitToCelsius(double fahrenheit) => (fahrenheit - 32) * 5 / 9;

    public static double DewPointFahrenheit(double temperatureF, double humidityPercent)
    {
        const double b = 17.62;
        const double c = 243.12;

        var tempC = FahrenheitToCelsius(temperatureF);
        var humidity = Math.Max(humidityPercent, 1);

        var gamma = Math.Log(humidity / 100) + b * tempC / (c + tempC);
        var dewPointC = c * gamma / (b - gamma);

        return dewPointC * 9 / 5 + 32;
    }
}
