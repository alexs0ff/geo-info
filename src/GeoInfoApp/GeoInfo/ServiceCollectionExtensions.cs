using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GeoInfoApp.GeoInfo
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddGeoInfoServices(this IServiceCollection services)
		{
			services.AddTransient<GeoInfoComposer>();

			return services;
		}
	}
}
