using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

		public GeoInfoController(GeoInfoComposer geoInfoComposer, ILogger<GeoInfoController> logger)
		{
			_geoInfoComposer = geoInfoComposer;
			_logger = logger;
		}

		[HttpGet]
		[Route("api/geoInfo/{zipCode}")]
		[ProducesResponseType(typeof(GeoInfoDto),StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> GetGeoInfo([FromRoute] string zipCode)
		{
			try
			{
				var geoInfo = await _geoInfoComposer.ComposeByZip(zipCode);

				return Ok(geoInfo);
			}
			catch (Exception e)
			{
				_logger.LogError(e, "An error while zip code request {zipCode}", zipCode);
				return StatusCode(StatusCodes.Status500InternalServerError);
			}
		}
	}
}
