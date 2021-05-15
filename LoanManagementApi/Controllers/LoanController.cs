using LoanManagementApi.DAL;
using LoanManagementApi.DAL.DAO;
using LoanManagementApi.DAL.Services;
using LoanManagementApi.Extentions;
using LoanManagementApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace LoanManagementApi.Controllers
{
	//TODO: add authorization
	[Route("api/[controller]")]
	[ApiController]
	public class LoanController : ControllerBase
	{
		private ILogger<LoanController> _logger;
		private ILoanDao _loanDao;
		private ICollateralManagement _collateralManagement;

		public LoanController(ILogger<LoanController> logger, ILoanDao dao, ICollateralManagement collateralManagement)
		{
			_logger = logger;
			_loanDao = dao;
			_collateralManagement = collateralManagement;
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
				return NotFound(new { error = $"no loan found by id: {id}" });
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

		//TODO: Remove this debug method
		[HttpPost("[action]")]
		public IActionResult Seed([FromServices] LoanDb db)
		{
			db.Seed();
			return Ok();
		}

		//TODO: Remove this debug method
		[Authorize]
		[HttpGet("[action]")]
		public IActionResult Authorized()
		{
			return Ok("data from authorized action");
		}

		/// <summary>
		/// Save loan and collaterals in their respective data sources.
		/// </summary>
		/// <param name="loanWithCollaterals">Json object containing both loan object and collaterals array</param>
		/// <returns></returns>
		/// <response code="200">Both, loan and collaterals where saved successfully</response>
		/// <response code="400">request body is invalid</response>
		/// <response code="500">something went wrong in CollateralManagementApi</response>
		/// <response code="503">unable to connect with CollateralManagementApi</response>
		[HttpPost("[action]")]
		public async Task<IActionResult> SaveWithCollaterals([FromBody] JsonElement loanWithCollaterals, [FromServices] LoanDb db)
		{
			if (loanWithCollaterals.ValueKind != JsonValueKind.Object)
				return BadRequest(new { error = "invalid request body" });

			JsonElement loanJson = loanWithCollaterals.GetProperty("loan");
			JsonElement collateralsJson = loanWithCollaterals.GetProperty("collaterals");

			if (collateralsJson.ValueKind != JsonValueKind.Array)
				return BadRequest(new { error = "invalid collateral array" });

			Loan loan = JsonSerializer.Deserialize<Loan>(loanJson.GetRawText(), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
			if (loan == null)
				return BadRequest(new { error = "invalid loan object" });

			HttpResponseMessage response;
			try { response = await _collateralManagement.Save(collateralsJson); }
			catch (HttpRequestException) { return StatusCode((int)HttpStatusCode.ServiceUnavailable, new { error = "unable to connect with CollateralManagementApi" }); }

			if (response.StatusCode != HttpStatusCode.MultiStatus)
				return StatusCode((int)HttpStatusCode.InternalServerError, new { error = "something went wrong in CollateralManagentApi, status code: "+response.StatusCode });

			int rowsAffected = _loanDao.Save(loan, db);

			if (rowsAffected <= 0)
				return StatusCode((int)HttpStatusCode.InternalServerError, new { error = "something went wrong while saving the loan details" });

			JsonElement responseBody = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;

			if (!responseBody.TryGetProperty("statuses", out JsonElement collateralSaveStatuses) || collateralSaveStatuses.ValueKind != JsonValueKind.Array)
				return StatusCode((int)HttpStatusCode.InternalServerError, new { error = "invalid response from CollateralManagementApi" });

			_logger.LogInformation(JsonSerializer.Serialize(new { loanSaveStatus = (int)HttpStatusCode.Created, collateralSaveStatuses = collateralSaveStatuses }));

			return Ok(new { loanSaveStatus = (int)HttpStatusCode.Created, collateralSaveStatuses = collateralSaveStatuses });
		}
	}
}