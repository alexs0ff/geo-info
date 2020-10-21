using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GeoInfoApp.GeoInfo
{
	public static class ZipCodeValidator
	{
		private static readonly Regex _regex = new Regex("\\d+[,]{1}[a-zA-Z]+");

		public static bool IsValid(string zipCode)
		{
			return _regex.IsMatch(zipCode);
		}
	}
}
