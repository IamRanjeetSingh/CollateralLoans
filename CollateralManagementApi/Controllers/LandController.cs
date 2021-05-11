using CollateralManagementApi.DAL;
using CollateralManagementApi.DAL.DAO;
using CollateralManagementApi.Models;
using CollateralManagementApi.Extentions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CollateralManagementApi.Controllers
{

	[ApiExplorerSettings(IgnoreApi = true)]
	[Route("api/[controller]")]
	[ApiController]
	public class LandController : ControllerBase
	{
		private ISubCollateralDao<Land> _dao;

		public LandController(ISubCollateralDao<Land> dao)
		{
			_dao = dao;
		}

		[HttpPost("")]
		public IActionResult Save([FromBody] Land land, [FromServices] CollateralDb db)
		{
			if (land == null)
				return StatusCode((int)HttpStatusCode.BadRequest, new { error = "cannot store null entity" });

			int rowId = _dao.Save(land, db);
			return CreatedAtAction(nameof(CollateralController.GetById), nameof(CollateralController).RemoveSuffix("Controller"), new { id = rowId }, land);
		}
	}
}
