using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GeoInfoApp.GoogleTimeZone;
using GeoInfoApp.OpenWeatherMap;
using Microsoft.Extensions.Logging;
using GoogleTimeZoneInfo = GeoInfoApp.GoogleTimeZone.TimeZoneInfo;

namespace GeoInfoApp.GeoInfo
{
	public class GeoInfoComposer
	{
		private readonly OpenWeatherMapClient _openWeatherMapClient;

		private readonly ILogger<GeoInfoComposer> _logger;

		private readonly GoogleTimeZoneClient _googleTimeZoneClient;

		public GeoInfoComposer(OpenWeatherMapClient openWeatherMapClient, ILogger<GeoInfoComposer> logger, GoogleTimeZoneClient googleTimeZoneClient)
		{
			_openWeatherMapClient = openWeatherMapClient;
			_logger = logger;
			_googleTimeZoneClient = googleTimeZoneClient;
		}

		public async Task<GeoInfoDto> ComposeByZip(string zipCode, CancellationToken cancellationToken = default)
		{
			_logger.LogInformation("Start composing geo information by {zipCode}", zipCode);

			WeatherInfo weatherInfo;
			GoogleTimeZoneInfo timeZoneInfo;
			try
			{
				weatherInfo = await _openWeatherMapClient.GetWeatherInfoByZip(zipCode, cancellationToken);
				timeZoneInfo = await GetTimeZoneInfo(weatherInfo, cancellationToken);
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
				TimeZone = timeZoneInfo.TimeZoneName
			};
		}

		private async Task<GoogleTimeZoneInfo> GetTimeZoneInfo(WeatherInfo weatherInfo, CancellationToken cancellationToken = default)
		{
			var lat = weatherInfo?.Coord?.Lat;
			var lon = weatherInfo?.Coord?.Lon;

			if (lat==null || lon==null)
			{
				throw new FormatException("The Coord property doesn`t have correct format");
			}

			_logger.LogInformation("start getting information about time zone from coord: {lat}, {lon}", lat, lon);

			var timeZone = await _googleTimeZoneClient.GetTimeZoneInfo(lat.Value, lon.Value, DateTimeOffset.UtcNow, cancellationToken);

			if (timeZone.Status!=GoogleTimeZoneInfoStatus.OK)
			{
				throw new Exception("Google time zone status is not correct:"+ timeZone.Status);
			}

			return timeZone;
		}
	}
}
