using WeatherApp.Services;

namespace WeatherApp.Controllers
{
    public partial class WeatherController
    {
        public class WeatherComparisonResult
        {
            public List<WeatherData>? WeatherDataList { get; set; }
            public double AverageTemperature { get; set; }
        }

    }
}
