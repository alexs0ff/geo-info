using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeoInfoApp.OpenWeatherMap;
using Microsoft.Extensions.Logging;

namespace GeoInfoApp.GeoInfo
{
	public class GeoInfoComposer
	{
		private readonly OpenWeatherMapClient _openWeatherMapClient;

		private readonly ILogger<GeoInfoComposer> _logger;

		public GeoInfoComposer(OpenWeatherMapClient openWeatherMapClient, ILogger<GeoInfoComposer> logger)
		{
			_openWeatherMapClient = openWeatherMapClient;
			_logger = logger;
		}

		public async Task<GeoInfoDto> ComposeByZip(string zipCode)
		{
			_logger.LogInformation("Start composing geo information by {zipCode}", zipCode);

			WeatherInfo weatherInfo;
			try
			{
				var weatherTask = _openWeatherMapClient.GetWeatherInfoByZip(zipCode);

				weatherInfo = await weatherTask;
			}
			catch (Exception e)
			{
				_logger.LogError(e, "an error occurs when loading geo information by {zipCode}", zipCode);
				throw;
			}

			return new GeoInfoDto
			{
				City = weatherInfo.Name,
				CurrentTemperatureCelsius = weatherInfo.Main.Temp,
				TimeZone = "test timezone"
			};
		}
	}
}
