using GeoInfoApp.History;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GeoInfoApp.History
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddHistoryServices(this IServiceCollection services)
		{
			services.AddTransient<IGeoInfoHistoryService, GeoInfoHistoryService>();

			return services;
		}
	}
}
