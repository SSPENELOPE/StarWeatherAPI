using react_weatherapp.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace react_weatherapp.Controllers
{
    public interface IWeatherApiService
    {
        Task<string> FetchWeatherByCityName(string cityName);
    }
    public class WeatherApiService : IWeatherApiService
    {
        private readonly HttpClient client;
        private readonly IOptions<OpenWeatherApiKey> _openWeatherApiKey;

        public WeatherApiService(IOptions<OpenWeatherApiKey> openWeatherApiKey, IHttpClientFactory clientFactory)
        {
            client = clientFactory.CreateClient("PublicWeatherApi");
            _openWeatherApiKey = openWeatherApiKey;
        }

        public async Task<string> FetchWeatherByCityName(string cityName)
        {
            var apiKey = _openWeatherApiKey.Value.ApiKey;
            string apiUrl = $"/data/2.5/weather?q={cityName}&units=imperial&exclude=hourly,daily&appid={apiKey}";
            var response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var stringResponse = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(stringResponse);
                var lat = json["coord"]["lat"].Value<string>();
                var lon = json["coord"]["lon"].Value<string>();
                var latLon = new LatLon { Lat = lat, Lon = lon };

                return await FetchWeatherByLatLon(latLon);
            }
            else
            {
                throw new HttpRequestException(response.ReasonPhrase);
            }
        }

        public async Task<string> FetchWeatherByLatLon(LatLon latLon)
        {
            var apiKey = _openWeatherApiKey.Value.ApiKey;

            string apiUrl = string.Format("/data/2.5/onecall?lat={0}&lon={1}&units=imperial&appid={2}", latLon.Lat, latLon.Lon, apiKey);
            var response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var stringResponse = await response.Content.ReadAsStringAsync();
                return stringResponse;
            }
            else
            {
                throw new HttpRequestException(response.ReasonPhrase);
            }
        }
    }
}