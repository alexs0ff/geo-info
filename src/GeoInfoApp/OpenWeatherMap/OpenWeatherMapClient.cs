using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace GeoInfoApp.OpenWeatherMap
{
	public class OpenWeatherMapClient
	{
		private static readonly Uri baseAddress = new Uri("https://api.openweathermap.org"); 

		private readonly HttpClient _httpClient;

		private readonly OpenWeatherMapOptions _openWeatherMapOptions;

		public OpenWeatherMapClient(HttpClient httpClient, IOptions<OpenWeatherMapOptions> openWeatherMapOptions)
		{
			_httpClient = httpClient;
			_openWeatherMapOptions = openWeatherMapOptions.Value;
			_httpClient.BaseAddress = baseAddress;
		}

		public async Task<WeatherInfo> GetWeatherInfoByZip(string zip, CancellationToken cancellationToken = default)
		{
			var response = await _httpClient.GetAsync(MakeZipUrl(zip), cancellationToken);

			response.EnsureSuccessStatusCode();

			await using var responseStream = await response.Content.ReadAsStreamAsync();
			var result = await JsonSerializer.DeserializeAsync<WeatherInfo>(responseStream, new JsonSerializerOptions
			{
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase
			}, cancellationToken);

			return result;
		}

		private string MakeZipUrl(string zip)
		{
			return $"/data/2.5/weather?zip={zip}&appid={_openWeatherMapOptions.ApiKey}&units={_openWeatherMapOptions.Units}";
		}
	}
}
