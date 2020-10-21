using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;

namespace GeoInfoApp.OpenWeatherMap
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddOpenWeatherMapClient(this IServiceCollection services)
		{
			services.AddHttpClient<OpenWeatherMapClient>()
				.AddTransientHttpErrorPolicy(p => p.RetryAsync(3));
			return services;
		}

		public static IServiceCollection AddOpenWeatherMapOptions(this IServiceCollection services, IConfiguration config)
		{
			services.Configure<OpenWeatherMapOptions>(
				config.GetSection(OpenWeatherMapOptions.OpenWeatherMap));
			return services;
		}
	}
}
