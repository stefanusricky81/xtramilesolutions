using WeatherApp.Api.Services;
using Xunit;

namespace WeatherApp.Api.Tests;

public class TemperatureConverterTests
{
    [Theory]
    [InlineData(32, 0)]
    [InlineData(212, 100)]
    [InlineData(-40, -40)]
    [InlineData(98.6, 37)]
    [InlineData(53.6, 12)]
    public void FahrenheitToCelsius(double fahrenheit, double expected)
    {
        var result = TemperatureConverter.FahrenheitToCelsius(fahrenheit);

        Assert.Equal(expected, result, 2);
    }

    [Fact]
    public void Gettemperature()
    {
        var result = TemperatureConverter.DewPointFahrenheit(68, 100);

        Assert.Equal(68.0, result, 1);
    }
}
