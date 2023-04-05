using Microsoft.AspNetCore.Mvc;
using react_weatherapp.Controllers;

namespace react_weatherapp.Controllers
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GetWeatherCity : Controller
    {
        private readonly IWeatherApiService _weatherApiService;

        public GetWeatherCity(IWeatherApiService weatherApiService)
        {
            _weatherApiService = weatherApiService;
        }

        [HttpGet]
        public async Task<IActionResult> GetWeather([FromQuery] string cityName)
        {
            // Call the WeatherApiService to fetch weather data
            var weatherData = await _weatherApiService.FetchWeatherByCityName(cityName);

            // Return the weather data as JSON response
            return Ok(weatherData);
        }
    }
}
