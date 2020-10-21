using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoInfoApp.History
{
	public class GeoInfoHistoryDto
	{
		public int Id { get; set; }

		public DateTime DateTimeUtc { get; set; }

		public string City { get; set; }

		public float CurrentTemperatureCelsius { get; set; }

		public string TimeZone { get; set; }
	}
}
