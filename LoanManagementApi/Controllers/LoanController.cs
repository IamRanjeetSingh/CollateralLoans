using LoanManagementApi.DAL;
using LoanManagementApi.DAL.DAO;
using LoanManagementApi.DAL.Services;
using LoanManagementApi.Extentions;
using LoanManagementApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace LoanManagementApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LoanController : ControllerBase
	{
		private ILogger<LoanController> _logger;
		private ILoanDao _loanDao;
		private ICollateralManagement _collateralManagement;

		public LoanController(ILogger<LoanController> logger, ILoanDao dao, ICollateralManagement collateralDao)
		{
			_logger = logger;
			_loanDao = dao;
			_collateralManagement = collateralDao;
		}

		[HttpGet("")]
		public IActionResult GetAll([FromQuery] Page page, [FromQuery] Filter filter, [FromServices] LoanDb db)
		{
			if (page == null || page.PageNo < 1 || page.PageSize < 1)
				return StatusCode((int)HttpStatusCode.BadRequest, new { error = "invalid page details" });

			return Ok(_loanDao.GetAll(page, filter, db));
		}

		[HttpGet("{id}")]
		public IActionResult GetById(int id, [FromServices] LoanDb db)
		{
			Loan loan = _loanDao.GetById(id, db);
			if (loan == null)
				return StatusCode((int)HttpStatusCode.NotFound, new { error = $"no entity found by id: {id}" });
			return Ok(loan);
		}

		[HttpPost("")]
		public IActionResult Save([FromBody] Loan loan, [FromServices] LoanDb db)
		{
			if (loan == null)
				return StatusCode((int)HttpStatusCode.BadRequest, new { error = "cannot store null entity" });

			int rowId = _loanDao.Save(loan, db);
			if (rowId == 0)
				return StatusCode((int)HttpStatusCode.InternalServerError, new { error = "error occurred while saving loan" });
			return CreatedAtAction(nameof(LoanController.GetById), nameof(LoanController).RemoveSuffix("Controller"), new { id = rowId }, loan);
		}

		[HttpPut("{id}")]
		public IActionResult UpdateFull(int id, [FromBody] Loan loan, [FromServices] LoanDb db)
		{
			if (loan == null)
				return StatusCode((int)HttpStatusCode.BadRequest, new { error = "cannot update with null entity" });

			int rowsAfected = _loanDao.UpdateFull(id, loan, db);
			if (rowsAfected <= 0)
				return StatusCode((int)HttpStatusCode.InternalServerError, new { error = "error occurred while fully updaing loan" });
			return Ok();
		}

		[HttpPatch("{id}")]
		public IActionResult UpdatePartial(int id, [FromBody] dynamic loan, [FromServices] LoanDb db)
		{
			int rowsAffected = _loanDao.UpdatePartial(id, loan, db);
			if (rowsAffected <= 0)
				return StatusCode((int)HttpStatusCode.InternalServerError, new { error = "error occurred while partially updating loan" });
			return Ok();
		}

		[HttpDelete("{id}")]
		public IActionResult Delete(int id, [FromServices] LoanDb db)
		{
			int rowsAffected = _loanDao.Delete(id, db);
			if (rowsAffected <= 0)
				return StatusCode((int)HttpStatusCode.InternalServerError, new { error = "error occurred while deleting loan" });
			return Ok();
		}

		[HttpPost("collateral")]
		public async Task<IActionResult> SaveCollaterals([FromBody] JsonElement collateralsJson)
		{
			if (collateralsJson.ValueKind != JsonValueKind.Array)
				return BadRequest(new { error = "Invalid Collateral Array" });

			HttpResponseMessage response = await _collateralManagement.Save(collateralsJson);
			JsonElement responseBody = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;
			return StatusCode((int)response.StatusCode, responseBody);
		}
	}
}