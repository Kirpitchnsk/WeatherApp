using Microsoft.AspNetCore.Mvc;
using WeatherApp.Services;

namespace WeatherApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public partial class WeatherController : ControllerBase
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
                _weatherService.GetLogger().LogError("Ошибка при получении погоды для города {City}", city);
                //return NotFound($"Weather data for city '{city}' not found");
            }
            return new WeatherData();
        }

        [HttpGet("compare")]
        public async Task<IActionResult> CompareWeather(string city1, string city2)
        {
            var message = string.Empty;

            if (string.IsNullOrWhiteSpace(city1) || string.IsNullOrWhiteSpace(city2))
            {
                message = "Ошибка в сравнении городов";
                _weatherService.GetLogger().LogInformation(message);
                return BadRequest(message);
            }

            var weatherData1 = await _weatherService.GetWeatherAsync(city1);
            var weatherData2 = await _weatherService.GetWeatherAsync(city2);

            if (weatherData1 == null || weatherData2 == null)
            {
                message = "Оба города не найдены";
                _weatherService.GetLogger().LogInformation(message);
                return NotFound(message);
            }

            var averageTemperature = (weatherData1.Temperature + weatherData2.Temperature) / 2;

            var result = new WeatherComparisonResult
            {
                WeatherDataList = new List<WeatherData> { weatherData1, weatherData2 },
                AverageTemperature = averageTemperature
            };

            message = $"Успешно сравнены 2 города {city1} и {city2}";
            _weatherService.GetLogger().LogInformation(message);
            return Ok(result);
        }

    }
}
