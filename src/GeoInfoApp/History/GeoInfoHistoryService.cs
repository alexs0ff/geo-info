using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GeoInfoApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GeoInfoApp.History
{
	public class GeoInfoHistoryService: IGeoInfoHistoryService
	{
		private readonly ILogger<GeoInfoHistoryService> _logger;

		private readonly GeoAppDbContext _appDbContext;

		public GeoInfoHistoryService(ILogger<GeoInfoHistoryService> logger, GeoAppDbContext appDbContext)
		{
			_logger = logger;
			_appDbContext = appDbContext;
		}

		public async Task<int> AddHistory(GeoInfoHistoryDto dto, CancellationToken cancellationToken = default)
		{
			_logger.LogInformation("Start add new history item: {item}", dto);
			var item = new GeoInfoHistoryItem
			{
				CurrentTemperatureCelsius = dto.CurrentTemperatureCelsius,
				TimeZone = dto.TimeZone,
				City = dto.City,
				DateTimeUtc = dto.DateTimeUtc
			};

			await _appDbContext.GeoInfoHistoryItems.AddAsync(item, cancellationToken);

			await _appDbContext.SaveChangesAsync(cancellationToken);

			return item.Id;
		}

		public async Task<IEnumerable<GeoInfoHistoryDto>> GetAllHistory( CancellationToken cancellationToken = default)
		{
			return await _appDbContext.GeoInfoHistoryItems.AsNoTracking().Select(i => new GeoInfoHistoryDto
			{
				CurrentTemperatureCelsius = i.CurrentTemperatureCelsius,
				TimeZone = i.TimeZone,
				City = i.City,
				DateTimeUtc = i.DateTimeUtc,
				Id = i.Id
			}).ToArrayAsync(cancellationToken);
		}
	}
}
