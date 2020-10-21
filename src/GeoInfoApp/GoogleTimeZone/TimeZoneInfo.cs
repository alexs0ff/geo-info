using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GeoInfoApp.GoogleTimeZone
{
	public class TimeZoneInfo
	{
		public string ErrorMessage { get; set; }

		[JsonConverter(typeof(JsonStringEnumConverter))]

		public GoogleTimeZoneInfoStatus Status { get; set; }

		public string TimeZoneName { get; set; }

		public string TimeZoneId { get; set; }
	}
}
