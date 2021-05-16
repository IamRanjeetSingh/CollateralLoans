using CollateralLoanMVC.Exceptions;
using CollateralLoanMVC.Models;
using CollateralLoanMVC.Services;
using CollateralLoanMVC.Util;
using CollateralLoanMVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace CollateralLoanMVC.Controllers
{
	[Authorize]
	[Route("[controller]")]
	public class LoanController : Controller
	{
		/// <summary>
		/// Used for communicating with the Loan Management Api.
		/// </summary>
		private readonly ILoanManagement _loanManagement;

		/// <summary>
		/// Used for communicating with the Risk Assessment Api.
		/// </summary>
		private readonly IRiskAssessment _riskAssessment;
		private readonly ICollateralManagement _collateralManagement;
		private readonly ILogger<LoanController> _logger;

		public LoanController(ILoanManagement loanManagement, IRiskAssessment riskAssessment, ICollateralManagement collateralManagement, ILogger<LoanController> logger)
		{
			_loanManagement = loanManagement;
			_riskAssessment = riskAssessment;
			_collateralManagement = collateralManagement;
			_logger = logger;
		}

		/// <summary>
		/// Get the page for creating a new loan instance. 
		/// </summary>
		/// <returns>page for new loan</returns>
		[HttpGet("[action]")]
		public ActionResult New()
		{
			ViewBag.LoanTypes = new string[] { "Home", "Car" };
			ViewBag.CollateralTypes = new string[] { "RealEstate", "Land" };
			return View("New");
		}

		//TODO: add post action method for creating new loan
		[HttpPost("[action]")]
		public async Task<IActionResult> New(IFormCollection form, [FromServices] ILoanManagement loanManagement)
		{
			JsonElement loanJson = JsonDocument.Parse(FormReader.GetLoanJson(form)).RootElement;
			JsonElement collateralsJson = JsonDocument.Parse($"[{FormReader.GetCollateralJson(form, _logger)}]").RootElement;

			_logger.LogInformation(collateralsJson.GetRawText());

			try
			{
				//return Ok(await loanManagement.SaveWithCollaterals(loanJson, collateralsJson));
				if (await loanManagement.SaveWithCollaterals(loanJson, collateralsJson))
				{
					int newLoanId = FormReader.GetLoan(form).Id;
					return RedirectToAction(actionName: nameof(LoanController.ViewLoan), new { id = newLoanId });
				}
				//return Ok("loan and collaterals saved successfully");
				else
					return StatusCode((int)HttpStatusCode.InternalServerError, new { error = "error occurred while saving loan and collaterals" });
			}
			catch (HttpRequestException) { return StatusCode((int)HttpStatusCode.ServiceUnavailable, new { error = "cannot connect with LoanManagementApi" }); }
			catch (UnexpectedResponseException e) { return StatusCode((int)HttpStatusCode.InternalServerError, new { error = e.Message }); }
		}

		/// <summary>
		/// Get a page for viewing an individual loan in more detailed manner.
		/// </summary>
		/// <param name="loanId">id of the loan to be viewed</param>
		/// <returns>page for viewing an individual loan</returns>
		[HttpGet("{id}")]
		public async Task<ActionResult> ViewLoan(int id)
		{
			Task<Loan> loanTask = _loanManagement.Get(id);
			Task<Risk> riskTask = _riskAssessment.Get(id);
			Task<List<Collateral>> collateralsTask = _collateralManagement.GetByLoanId(id);

			Loan loan;
			Risk risk;
			Collateral collateral;

			try { loan = await loanTask; }
			catch (HttpRequestException) { return StatusCode((int)HttpStatusCode.ServiceUnavailable, new { error = "unable to connect with LoanManagementApi" }); }
			catch (UnexpectedResponseException) { return StatusCode((int)HttpStatusCode.InternalServerError, new { error = "something went wrong in LoanManagementApi" }); }

			try { risk = await riskTask; }
			catch (HttpRequestException) { return StatusCode((int)HttpStatusCode.ServiceUnavailable, new { error = "unable to connect with RiskAssessmentApi" }); }
			catch (UnexpectedResponseException) { return StatusCode((int)HttpStatusCode.InternalServerError, new { error = "something went wrong in RiskAssessmentApi" }); }

			try { collateral = (await collateralsTask)[0]; }
			catch (HttpRequestException) { return StatusCode((int)HttpStatusCode.ServiceUnavailable, new { error = "unable to connect with CollateralManagementApi" }); }
			catch (UnexpectedResponseException) { return StatusCode((int)HttpStatusCode.InternalServerError, new { error = "something went wrong in CollateralManagementApi" }); }

			if (loan == null)
				return NotFound();

			return View("ViewLoan",
				new ViewLoanViewModel()
				{
					Loan = loan,
					Risk = risk,
					Collateral = collateral
				}
			);
		}

		[HttpGet("[action]/{id}")]
		public async Task<ActionResult> ViewCollateral(int id)
		{
			var collateralJson = await _collateralManagement.GetCollateral(id);
			_logger.LogInformation(collateralJson.GetRawText());

			JsonElement type;
			if (!collateralJson.TryGetProperty(nameof(Models.Collateral.Type).Trim(), out type) && !collateralJson.TryGetProperty(nameof(Models.Collateral.Type).ToLower().Trim(), out type))
				throw new UnexpectedResponseException("no type property found");

			string collateralType = type.GetString();

			JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
			if (collateralType.ToLower() == nameof(Land).ToLower())
			{
				Land land = JsonSerializer.Deserialize<Land>(collateralJson.GetRawText(), jsonSerializerOptions);
				return View("ViewCollateral", new ViewCollateralViewModel { Land = land });
				//return Ok(new { collateral = land });
			}
			else
			{
				RealEstate realEstate = JsonSerializer.Deserialize<RealEstate>(collateralJson.GetRawText(), jsonSerializerOptions);
				return View("ViewCollateral", new ViewCollateralViewModel { RealEstate = realEstate });
				//return Ok(new { collateral = realEstate });
			}
		}

		/// <summary>
		/// Get a list of <see cref="Loan"/>, filtered and paginated.
		/// </summary>
		/// <param name="page">page details</param>
		/// <param name="filter">filters to be applied</param>
		/// <returns>list of <see cref="Loan"/></returns>
		/// <response code="200">list of <see cref="Loan"/></response>
		/// <response code="503">cannot communicate with LoanManagementApi</response>
		/// <response code="500">something went wrong in LoanManagementApi</response>
		[HttpGet("[action]")]
		public async Task<IActionResult> List([FromQuery] Page page, [FromQuery] LoanFilter filter)
		{
			List<Loan> loans;

			try { loans = await _loanManagement.GetAll(page, filter); }
			catch (HttpRequestException) { return StatusCode((int)HttpStatusCode.ServiceUnavailable, new { error = "LoanManagementApi unavailable" }); }
			catch (UnexpectedResponseException) { return StatusCode((int)HttpStatusCode.InternalServerError, new { error = "something went wrong in LoanManagementApi" }); }

			return Ok(loans);
		}


		[HttpPost("[action]")]
		public async Task<IActionResult> Test(IFormCollection form, [FromServices] ILoanManagement loanManagement)
		{
			JsonElement loanJson = JsonDocument.Parse(FormReader.GetLoanJson(form)).RootElement;
			JsonElement collateralsJson = JsonDocument.Parse($"[{FormReader.GetCollateralJson(form, _logger)}]").RootElement;

			_logger.LogInformation(collateralsJson.GetRawText());

			try 
			{
				//return Ok(await loanManagement.SaveWithCollaterals(loanJson, collateralsJson));
				if (await loanManagement.SaveWithCollaterals(loanJson, collateralsJson))
				{
					int newLoanId = FormReader.GetLoan(form).Id;
					return RedirectToAction(actionName: nameof(LoanController.ViewLoan), new { id = newLoanId });
				}
				//return Ok("loan and collaterals saved successfully");
				else
					return StatusCode((int)HttpStatusCode.InternalServerError, new { error = "error occurred while saving loan and collaterals" });
			}
			catch(HttpRequestException) { return StatusCode((int)HttpStatusCode.ServiceUnavailable, new { error = "cannot connect with LoanManagementApi" }); }
			catch (UnexpectedResponseException e) { return StatusCode((int)HttpStatusCode.InternalServerError, new { error = e.Message }); }
		}
	}
}