using Microsoft.AspNetCore.Mvc;
using WeatherApp.Services;

namespace WeatherApp.Controllers
{    
    [ApiController]
    [Route("[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly WeatherService _weatherService;

        public WeatherController(WeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [HttpGet("{city}")]
        public async Task<WeatherData> GetWeather(string city)
        {
            var message = string.Empty;

            if (!ModelState.IsValid)
            {
                message = $"Ошибка при обработке города {city}";
                _weatherService.GetLogger().LogError(message);
                //return BadRequest("City parameter is required");
            }

            if (string.IsNullOrEmpty(city))
            {
                message = $"Ошибка город не может быть пустым {city}";
                _weatherService.GetLogger().LogError(message);
            }

            var weatherData = await _weatherService.GetWeatherAsync(city);

            if (weatherData != null)
            {
                message = $"Успешное получение информации о городе {city}";
                _weatherService.GetLogger().LogInformation(message);
                //return Ok(weatherData);
                return weatherData;
            }
            else
            {
                // Если что-то пошло не так, можно залогировать ошибку
                _weatherService.GetLogger().LogError("Ошибка при получении погоды для города {City}", city);
                //return NotFound($"Weather data for city '{city}' not found");
            }
            return new WeatherData();
        }

        public class WeatherComparisonResult
        {
            public List<WeatherData>? WeatherDataList { get; set; }
            public double AverageTemperature { get; set; }
        }

        [HttpGet("compare")]
        public async Task<IActionResult> CompareWeather(string city1, string city2)
        {
            if (string.IsNullOrWhiteSpace(city1) || string.IsNullOrWhiteSpace(city2))
            {
                return BadRequest("Both city parameters are required");
            }

            var weatherData1 = await _weatherService.GetWeatherAsync(city1);
            var weatherData2 = await _weatherService.GetWeatherAsync(city2);

            if (weatherData1 == null || weatherData2 == null)
            {
                _weatherService.GetLogger().LogInformation(message);
                return NotFound("Weather data not found for one or both cities");
            }

            var averageTemperature = (weatherData1.Temperature + weatherData2.Temperature) / 2;

            var result = new WeatherComparisonResult
            {
                WeatherDataList = new List<WeatherData> { weatherData1, weatherData2 },
                AverageTemperature = averageTemperature
            };

            return Ok(result);
        }

    }
}
