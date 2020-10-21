using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GeoInfoApp.History
{
	public interface IGeoInfoHistoryService
	{
		Task<int> AddHistory(GeoInfoHistoryDto dto, CancellationToken cancellationToken = default);
		Task<IEnumerable<GeoInfoHistoryDto>> GetAllHistory( CancellationToken cancellationToken = default);
	}
}
