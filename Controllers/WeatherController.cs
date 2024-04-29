using Microsoft.AspNetCore.Mvc;
using WeatherApp.Services;

namespace WeatherApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly WeatherService _weatherService;

        public WeatherController(WeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [HttpGet("{city}")]
        public async Task<IActionResult> GetWeather(string city)
        {
            var weatherData = await _weatherService.GetWeatherAsync(city);
            if (weatherData != null)
            {
                return Ok(weatherData);
            }
            else
            {
                return NotFound(); // Возвращаем 404 Not Found, если данные о погоде не найдены
            }
        }
    }
}
