using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoInfoApp.OpenWeatherMap
{
	public class WeatherInfo
	{
		public string Name { get; set; }

		public WeatherInfoMain Main { get; set; }

		public WeatherInfoCoord Coord { get; set; }
	}
}
