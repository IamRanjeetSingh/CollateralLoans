using CollateralLoanMVC.Exceptions;
using CollateralLoanMVC.Models;
using CollateralLoanMVC.Services;
using CollateralLoanMVC.Util;
using CollateralLoanMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace CollateralLoanMVC.Controllers
{
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

		public LoanController(ILoanManagement loanManagement, IRiskAssessment riskAssessment)
		{
			_loanManagement = loanManagement;
			_riskAssessment = riskAssessment;
		}

		//TODO: check integration
		/// <summary>
		/// Get the page for creating a new loan instance. 
		/// </summary>
		/// <returns>page for new loan</returns>
		[HttpGet("[action]")]
		public ActionResult New()
		{
			return View();
		}

		//TODO: add post action method for creating new loan

		/// <summary>
		/// Get a page for viewing an individual loan in more detailed manner.
		/// </summary>
		/// <param name="loanId">id of the loan to be viewed</param>
		/// <returns>page for viewing an individual loan</returns>
		[HttpGet("{id}")]
		public async Task<ActionResult> View(int id)
		{
			Task<Loan> loanTask = _loanManagement.Get(id);
			Task<Risk> riskTask = _riskAssessment.Get(id);

			Loan loan;
			Risk risk;

			try { loan = await loanTask; }
			catch (HttpRequestException) { return StatusCode((int)HttpStatusCode.ServiceUnavailable, new { error = "unable to connect with LoanManagementApi" }); }
			catch (UnexpectedResponseException) { return StatusCode((int)HttpStatusCode.InternalServerError, new { error = "something went wrong in LoanManagementApi" }); }

			try { risk = await riskTask; }
			catch (HttpRequestException) { return StatusCode((int)HttpStatusCode.ServiceUnavailable, new { error = "unable to connect with RiskAssessmentApi" }); }
			catch (UnexpectedResponseException) { return StatusCode((int)HttpStatusCode.InternalServerError, new { error = "something went wrong in RiskAssessmentApi" }); }

			if (loan == null)
				return NotFound();

			return View(
				new LoanViewModel()
				{
					Loan = loan,
					Risk = risk
				}
			);
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
	}
}
