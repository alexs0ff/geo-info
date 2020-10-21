using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GeoInfoApp.History;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GeoInfoApp.GeoInfo
{
	[ApiController]
	public class GeoInfoController: ControllerBase
	{
		private readonly GeoInfoComposer _geoInfoComposer;

		private readonly ILogger<GeoInfoController> _logger;

		private readonly IGeoInfoHistoryService _geoInfoHistoryService;

		public GeoInfoController(GeoInfoComposer geoInfoComposer, ILogger<GeoInfoController> logger, IGeoInfoHistoryService geoInfoHistoryService)
		{
			_geoInfoComposer = geoInfoComposer;
			_logger = logger;
			_geoInfoHistoryService = geoInfoHistoryService;
		}

		[HttpGet]
		[Route("api/geoInfo/{zipCode}")]
		[ProducesResponseType(typeof(GeoInfoDto),StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> GetGeoInfo([FromRoute] string zipCode, CancellationToken cancellationToken)
		{
			if (!ZipCodeValidator.IsValid(zipCode))
			{
				return BadRequest();
			}

			try
			{
				var geoInfo = await _geoInfoComposer.ComposeByZip(zipCode, cancellationToken);

				await _geoInfoHistoryService.AddHistory(new GeoInfoHistoryDto
				{
					TimeZone = geoInfo.TimeZone,
					City = geoInfo.City,
					CurrentTemperatureCelsius = geoInfo.CurrentTemperatureCelsius,
					DateTimeUtc = DateTime.UtcNow
				}, cancellationToken);

				return Ok(geoInfo);
			}
			catch (Exception e)
			{
				_logger.LogError(e, "An error while zip code request {zipCode}", zipCode);
				return StatusCode(StatusCodes.Status500InternalServerError);
			}
		}

		[HttpGet]
		[Route("api/geoInfoHistory")]
		[ProducesResponseType(typeof(GeoInfoHistoryDto), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> GetHistory( CancellationToken cancellationToken)
		{
			try
			{
				var historyItems = await _geoInfoHistoryService.GetAllHistory(cancellationToken);

				return Ok(historyItems);
			}
			catch (Exception e)
			{
				_logger.LogError(e, "An error while history request request");
				return StatusCode(StatusCodes.Status500InternalServerError);
			}
		}
	}
}
