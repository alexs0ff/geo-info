using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GeoInfoApp.History
{
	public class GeoInfoHistoryItem
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public DateTime DateTimeUtc { get; set; }

		[Required]
		public string City { get; set; }

		[Required]
		public float CurrentTemperatureCelsius { get; set; }

		[Required]
		public string TimeZone { get; set; }
	}
}
