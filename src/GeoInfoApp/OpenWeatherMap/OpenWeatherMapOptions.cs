using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoInfoApp.OpenWeatherMap
{
	public class OpenWeatherMapOptions
	{
		public const string OpenWeatherMap = "OpenWeatherMap";

		public string ApiKey { get; set; }

		public string Units { get; set; }
	}
}
