using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
