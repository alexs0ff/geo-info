using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeoInfoApp.History;
using Microsoft.EntityFrameworkCore;

namespace GeoInfoApp.Data
{
	public class GeoAppDbContext: DbContext
	{
		public GeoAppDbContext(DbContextOptions options) : base(options)
		{
		}

		public DbSet<GeoInfoHistoryItem> GeoInfoHistoryItems { get; set; }
	}
}
