using System.IO;
using System.Text;

namespace GeoInfoApp.Extensions
{
	public static class StringExtensions
	{
		public static Stream ToStream(this string value)
		{
			return new MemoryStream(Encoding.UTF8.GetBytes(value));
		}
	}
}
