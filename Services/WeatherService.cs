using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace WeatherApp.Services
{
    public class WeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "e158fb25373f5ac6ef2df071c00ff75c"; // Ваш API ключ от openweathermap

        public WeatherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<WeatherData> GetWeatherAsync(string city)
        {
            string apiUrl = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={_apiKey}&units=metric";

            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string weatherJson = await response.Content.ReadAsStringAsync();
                WeatherData weatherData = ParseWeatherJson(weatherJson);
                return weatherData;
            }
            else
            {
                // Обработка ошибок, если запрос не успешен
                return null;
            }
        }

        public WeatherData ParseWeatherJson(string weatherJson)
        {
            JObject json = JObject.Parse(weatherJson);

            WeatherData weatherData = new WeatherData
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
