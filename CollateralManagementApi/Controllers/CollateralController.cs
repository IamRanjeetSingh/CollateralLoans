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

		[HttpGet("")]
		public IActionResult Get([FromQuery] Page page, [FromQuery] Filter filter, [FromServices] CollateralDb db)
		{
			if (page == null || page.PageNo < 1 || page.PageSize < 1)
				return StatusCode((int)HttpStatusCode.BadRequest, new { error = "invalid page details" });

			return Ok(_dao.GetAll(db, page, filter));
		}

		[HttpGet("{id}")]
		public IActionResult GetById(int id, [FromServices] CollateralDb db)
		{
			Collateral collateral = _dao.GetById(db, id);
			if (collateral == null)
				return StatusCode((int)HttpStatusCode.NotFound, new { error = $"no entity found by id: {id}" });
			return Ok(collateral);
		}

		[HttpPost("")]
		public IActionResult Save([FromBody] JsonElement collateralJson, [FromServices] CollateralDb db)
		{
			Collateral collateral = null;
			try { collateral = CollateralSerializer.DeserializeByType(collateralJson, "type"); }
			catch (ArgumentException e) { return BadRequest(new { error =  e.Message }); }

			int rowId = _dao.Save(collateral, db);
			if (rowId <= 0)
				return StatusCode((int)HttpStatusCode.InternalServerError, new { error = "error occurred while saving the collateral" });
			return CreatedAtAction(nameof(CollateralController.GetById), nameof(CollateralController).RemoveSuffix("Controller"), new { id = rowId }, collateral);
		}

		[HttpPost("collection")]
		public IActionResult SaveMultiple([FromBody] JsonElement collateralsJson, [FromServices] CollateralDb db)
		{
			if (collateralsJson.ValueKind != JsonValueKind.Array)
				return BadRequest(new { error = "Invalid Collateral Array" });

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
