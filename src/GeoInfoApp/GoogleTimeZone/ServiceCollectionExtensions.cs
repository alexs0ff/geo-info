using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;

namespace GeoInfoApp.GoogleTimeZone
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddGoogleTimeZoneClient(this IServiceCollection services)
		{
			services.AddHttpClient<GoogleTimeZoneClient>()
				.AddTransientHttpErrorPolicy(p => p.RetryAsync(3));
			return services;
		}

		public static IServiceCollection AddGoogleTimeZoneOptions(this IServiceCollection services, IConfiguration config)
		{
			services.Configure<GoogleTimeZoneOptions>(
				config.GetSection(GoogleTimeZoneOptions.GoogleTimeZone));
			return services;
		}
	}
}
