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
	[ApiController]
	[Route("api/[controller]")]
	public class RealEstateController : ControllerBase
	{
		private ISubCollateralDao<RealEstate> _dao;

		public RealEstateController(ISubCollateralDao<RealEstate> dao)
		{
			_dao = dao;
		}

		[HttpPost("")]
		public IActionResult Save([FromBody] RealEstate realEstate, [FromServices] CollateralDb db)
		{
			if (realEstate == null)
				return StatusCode((int)HttpStatusCode.BadRequest, new { error = "cannot store null entity" });

			int rowId = _dao.Save(realEstate, db);
			return CreatedAtAction(nameof(CollateralController.GetById), nameof(CollateralController).RemoveSuffix("Controller"), new { id = rowId }, realEstate);
		}
	}
}
