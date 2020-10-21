using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoInfoApp.OpenWeatherMap
{
	public class WeatherInfo
	{
		/// <summary>
		/// The city name.
		/// </summary>
		public string Name { get; set; }

		public WeatherInfoMain Main { get; set; }
	}
}
