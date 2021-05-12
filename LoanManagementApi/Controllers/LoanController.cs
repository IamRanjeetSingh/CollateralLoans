using LoanManagementApi.DAL;
using LoanManagementApi.DAL.DAO;
using LoanManagementApi.DAL.Services;
using LoanManagementApi.Extentions;
using LoanManagementApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
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

		/// <summary>
		/// Get a list of <see cref="Loan"/>, filtered and paginated.
		/// </summary>
		/// <param name="page">page details</param>
		/// <param name="filter">filters to be applied</param>
		/// <param name="db">data source to be searched</param>
		/// <returns>list of <see cref="Loan"/></returns>
		/// <response code="200">list of <see cref="Loan"/></response>
		[HttpGet("")]
		public IActionResult GetAll([FromQuery] Page page, [FromQuery] Filter filter, [FromServices] LoanDb db)
		{
			return Ok(_loanDao.GetAll(page, filter, db));
		}

		/// <summary>
		/// Get a single <see cref="Loan"/> instance associated with the given id.
		/// </summary>
		/// <param name="id">id of the loan to be fetched</param>
		/// <param name="db">data source to be searched</param>
		/// <returns>single loan instance associated with the given id</returns>
		/// <response code="200">single loan instance associated with the given id</response>
		/// <response code="404">no loan found for the given id</response>
		/// <response code="500">more than one loan found for the given id</response>
		[HttpGet("{id}")]
		public IActionResult GetById(int id, [FromServices] LoanDb db)
		{
			Loan loan;
			try { loan = _loanDao.GetById(id, db); }
			catch (InvalidOperationException) { return StatusCode((int)HttpStatusCode.InternalServerError, new { error = "more than one loan found for the given id" }); }
			
			if (loan == null) 
				return NotFound( new { error = $"no loan found by id: {id}" });
			return Ok(loan);
		}

		[HttpPost("")]
		public IActionResult Save([FromBody] Loan loan, [FromServices] LoanDb db)
		{
			if (loan == null)
				return StatusCode((int)HttpStatusCode.BadRequest, new { error = "cannot store null entity" });

			int rowId = _loanDao.Save(loan, db);
			return CreatedAtAction(nameof(LoanController.GetById), nameof(LoanController).RemoveSuffix("Controller"), new { id = rowId }, loan);
		}

		[HttpPut("{id}")]
		public IActionResult UpdateFull(int id, [FromBody] Loan loan, [FromServices] LoanDb db)
		{
			if (loan == null)
				return StatusCode((int)HttpStatusCode.BadRequest, new { error = "cannot update with null entity" });

			int rowsAffected = _loanDao.UpdateFull(id, loan, db);
			return Ok(new { rowsaffected = rowsAffected });
		}

		[HttpPatch("{id}")]
		public IActionResult UpdatePartial(int id, [FromBody] dynamic loan, [FromServices] LoanDb db)
		{
			int rowsAffected = _loanDao.UpdatePartial(id, loan, db);
			return Ok(new { rowsaffected = rowsAffected });
		}

		[HttpDelete("{id}")]
		public IActionResult Delete(int id, [FromServices] LoanDb db)
		{
			int rowsAffected = _loanDao.Delete(id, db);
			return Ok(new { rowsaffected = rowsAffected });
		}

		[HttpPost("collateral")]
		public async Task<IActionResult> SaveCollaterals([FromBody] JsonElement collateralsJson)
		{
			if (collateralsJson.ValueKind != JsonValueKind.Array)
				return BadRequest(new { error = "Invalid Collateral Array" });

			HttpResponseMessage response;
			try { response = await _collateralManagement.Save(collateralsJson); }
			catch (HttpRequestException) { return StatusCode((int) HttpStatusCode.ServiceUnavailable); }

			if (response.StatusCode != HttpStatusCode.OK) 
				return StatusCode((int)response.StatusCode);

			JsonElement responseBody = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;
			return Ok(responseBody);
		}


		//TODO: Remove this debug method
		[HttpGet("Test")]
		public IActionResult Test()
		{
			throw new System.Exception();
		}

		//TODO: Remove this debug method
		[HttpPost("[action]")]
		public IActionResult Seed([FromServices] LoanDb db)
		{
			db.Seed();
			return Ok();
		}
	}
}