using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using GeoInfoApp.Extensions;
using Microsoft.Extensions.Options;

namespace GeoInfoApp.GoogleTimeZone
{
	public class GoogleTimeZoneClient
	{
		private static readonly Uri baseAddress = new Uri("https://maps.googleapis.com");

		private readonly HttpClient _httpClient;

		private readonly GoogleTimeZoneOptions _googleTimeZoneOptions;

		public GoogleTimeZoneClient(HttpClient httpClient, IOptions<GoogleTimeZoneOptions> googleTimeZoneOptions)
		{
			_httpClient = httpClient;
			_httpClient.BaseAddress = baseAddress;
			_googleTimeZoneOptions = googleTimeZoneOptions.Value;
		}

		public async Task<TimeZoneInfo> GetTimeZoneInfo(float lat, float lng, DateTimeOffset timestamp, CancellationToken cancellationToken = default)
		{
			var apiQuery = MakeUrl(lat, lng, timestamp);

			var response = await _httpClient.GetAsync(apiQuery, cancellationToken);

			response.EnsureSuccessStatusCode();

			await using var responseStream = await response.Content.ReadAsStreamAsync();
			var result = await ParseResponse(responseStream);

			result = await ModifyForUnBillingRequests(result);

			return result;
		}

		private static async Task<TimeZoneInfo> ParseResponse(Stream responseStream)
		{
			var result = await JsonSerializer.DeserializeAsync<TimeZoneInfo>(responseStream, new JsonSerializerOptions
			{
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase
			});
			return result;
		}

		private string MakeUrl(float lat, float lng, DateTimeOffset timestamp)
		{
			var latRaw = lat.ToString(CultureInfo.InvariantCulture);
			var lngRaw = lng.ToString(CultureInfo.InvariantCulture);
			return
				$"/maps/api/timezone/json?location={latRaw},{lngRaw}&timestamp={timestamp.ToUnixTimeSeconds().ToString(CultureInfo.InvariantCulture)}&key={_googleTimeZoneOptions.ApiKey}";
		}

		private static async Task<TimeZoneInfo> ModifyForUnBillingRequests(TimeZoneInfo response)
		{
			var needBilling = !string.IsNullOrWhiteSpace(response.ErrorMessage) &&
			                  response.ErrorMessage.Contains("You must enable Billing");

			return (response.Status, needBilling) switch
			{
				(GoogleTimeZoneInfoStatus.REQUEST_DENIED, true) => await ParseResponse(TestResponse.ToStream()),
				_ => response
			};
		}

		private const string TestResponse = "{\r\n   \"dstOffset\" : 0,\r\n   \"rawOffset\" : -28800,\r\n   \"status\" : \"OK\",\r\n   \"timeZoneId\" : \"America/Los_Angeles\",\r\n   \"timeZoneName\" : \"Test Unbilling timezone\"\r\n}";
	}
}
