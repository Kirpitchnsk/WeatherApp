using Newtonsoft.Json.Linq;

namespace WeatherApp.Services
{
    public class WeatherService
    {
        HttpClient _httpClient;

        private readonly IConfiguration _configuration;

        private readonly ILogger<WeatherService> _logger;

        public WeatherService(HttpClient httpClient, IConfiguration configuration, ILogger<WeatherService> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
        }

        public ILogger<WeatherService> GetLogger()
        {
            return _logger;
        }

        public async Task<WeatherData> GetWeatherAsync(string city)
        {
            try
            {
                // Ваш код получения погоды
                _logger.LogInformation("Запрошена погода для города {City}", city);


                var apiKey = _configuration["WeatherApi:ApiKey"];

                var apiUrl = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric";

                var response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var weatherJson = await response.Content.ReadAsStringAsync();
                    var weatherData = ParseWeatherJson(weatherJson);
                    return weatherData;
                }
                else
                {
                    // Обработка ошибок, если запрос не успешен
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при выполнении метода GetWeatherAsync");
                throw; // Перебрасываем исключение выше для обработки в другом месте
            }

        }

        public WeatherData ParseWeatherJson(string weatherJson)
        {
            var json = JObject.Parse(weatherJson);

            var weatherData = new WeatherData
            {
                City = (string)json["name"],
                Temperature = (double)json["main"]["temp"],
                Pressure = (double)json["main"]["pressure"],
                Humidity = (double)json["main"]["humidity"],
                WindSpeed = (double)json["wind"]["speed"],
                Cloudiness = (string)json["weather"][0]["description"],
                CurrentTime = DateTimeOffset.FromUnixTimeSeconds((long)json["dt"]).DateTime,
                TimeDifference = DateTime.UtcNow - DateTimeOffset.FromUnixTimeSeconds((long)json["dt"])
            };

            return weatherData;
        }
    }
}
