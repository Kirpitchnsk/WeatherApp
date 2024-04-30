namespace WeatherApp.Services
{
    public class WeatherData
    {
        public string? City { get; set; }
        public double Temperature { get; set; }
        public double Pressure { get; set; }
        public double Humidity { get; set; }
        public double WindSpeed { get; set; }
        public string? Cloudiness { get; set; }
        public DateTime CurrentTime { get; set; }
        public TimeSpan TimeDifference { get; set; }
    }
}