/// <reference lib="es2015" />

class WeatherClient {
    private readonly apiUrl: string;
    private readonly apiKey: string;

    constructor(apiUrl: string, apiKey: string) {
        this.apiUrl = apiUrl;
        this.apiKey = apiKey;
    }

    async getWeather(city: string): Promise<string | null> {
        const apiUrl = `${this.apiUrl}/weather?q=${city}&appid=${this.apiKey}&units=metric`;

        try {
            const response = await fetch(apiUrl);

            if (!response.ok) {
                throw new Error('Failed to fetch weather data');
            }

            const weatherJson = await response.json();
            return JSON.stringify(weatherJson);
        } catch (error) {
            console.error('Error fetching weather data:', error);
            return null;
        }
    }
}

async function createWeatherClientAndGetWeather(city: string): Promise<string | null> {
    const weatherClient = new WeatherClient(`https://api.openweathermap.org/data/2.5`, `e158fb25373f5ac6ef2df071c00ff75c`);

    return await weatherClient.getWeather(city);
}
