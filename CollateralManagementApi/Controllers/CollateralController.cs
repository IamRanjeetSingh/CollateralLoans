using CollateralManagementApi.DAL;
using CollateralManagementApi.DAL.DAO;
using CollateralManagementApi.Extentions;
using CollateralManagementApi.Models;
using CollateralManagementApi.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;

namespace CollateralManagementApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CollateralController : ControllerBase
	{
		private ICollateralDao _dao;
		private ILogger<CollateralController> _logger;

		public CollateralController(ICollateralDao dao, ILogger<CollateralController> logger)
		{
			_dao = dao;
			_logger = logger;
		}

		/// <summary>
		/// Get a list of <see cref="Collateral"/>, paginated and filtered.
		/// </summary>
		/// <param name="page">page details</param>
		/// <param name="filter">value to filter the list upon</param>
		/// <param name="db">data source to be searched</param>
		/// <returns>list of <see cref="Collateral"/>. This list can be empty</returns>
		/// <response code="200">list of <see cref="Collateral"/></response>
		[HttpGet("")]
		public IActionResult Get([FromQuery] Page page, [FromQuery] Filter filter, [FromServices] CollateralDb db)
		{
			return Ok(_dao.GetAll(page, filter, db));
		}

		[HttpGet("{id}")]
		public IActionResult GetById(int id, [FromServices] CollateralDb db)
		{
			Collateral collateral = _dao.GetById(db, id);
			if (collateral == null)
				return StatusCode((int)HttpStatusCode.NotFound, new { error = $"no collateral found by id: {id}" });

			return Ok(collateral);
		}

		[HttpPost("")]
		public IActionResult Save([FromBody] JsonElement collateralJson, [FromServices] CollateralDb db)
		{
			Collateral collateral = null;
			try { collateral = CollateralSerializer.DeserializeByType(collateralJson, "type"); }
			catch (ArgumentException e) { return BadRequest(new { error =  e.Message }); }

			int rowId = _dao.Save(collateral, db);
			return CreatedAtAction(nameof(CollateralController.GetById), nameof(CollateralController).RemoveSuffix("Controller"), new { id = rowId }, collateral);
		}

		[HttpPost("collection")]
		public IActionResult SaveMultiple([FromBody] JsonElement collateralsJson, [FromServices] CollateralDb db)
		{
			if (collateralsJson.ValueKind != JsonValueKind.Array)
				return BadRequest(new { error = "provided content is not a collateral array" });

			List<int> statusCodes = new List<int>();
			for(int index = 0, length = collateralsJson.GetArrayLength(); index < length; index++)
			{
				JsonElement collateralJson = collateralsJson[index];
				Collateral collateral = null;
				
				try { collateral = CollateralSerializer.DeserializeByType(collateralJson, "type"); }
				catch(ArgumentException) { statusCodes.Add((int)HttpStatusCode.BadRequest); }

				_dao.Save(collateral, db);
				statusCodes.Add((int)HttpStatusCode.Created);
			}

			return StatusCode((int)HttpStatusCode.MultiStatus, new { statuses = statusCodes });
		}

		//(DEBUG) Used to populate the database with seed data.
		[HttpPost("[action]")]
		public IActionResult Seed([FromServices] CollateralDb db)
		{
			db.SeedData();
			return Ok();
		}
	}
}
