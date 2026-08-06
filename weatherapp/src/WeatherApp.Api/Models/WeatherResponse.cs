namespace WeatherApp.Api.Models;

public class WeatherResponse
{
    public string Location { get; set; } = "";
    public DateTime TimeUtc { get; set; }
    public double WindSpeedMph { get; set; }
    public string WindDirection { get; set; } = "";
    public double VisibilityMiles { get; set; }
    public string SkyConditions { get; set; } = "";
    public double TemperatureF { get; set; }
    public double TemperatureC { get; set; }
    public double DewPointF { get; set; }
    public double DewPointC { get; set; }
    public int RelativeHumidity { get; set; }
    public double PressureHpa { get; set; }
}
