using CollateralLoanMVC.Util;
using CollateralLoanMVC.Models;
using CollateralLoanMVC.Services;
using CollateralLoanMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;

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

		/// <summary>
		/// Used to get the view for creating a new loan instance. 
		/// </summary>
		/// <returns>view for new loan</returns>
		[HttpGet("[action]")]
		public ActionResult New()
		{
			return View();
		}

		//TODO: add post action method for creating new loan

		/// <summary>
		/// Used to get the view for viewing an individual loan in more detailed manner.
		/// </summary>
		/// <param name="loanId">Id of the loan to be viewed</param>
		/// <returns>view for viewing an individual loan</returns>
		[HttpGet("{id}")]
		public async Task<ActionResult> View(int id)
		{
			Task<Loan> loanTask = _loanManagement.Get(id);
			Task<Risk> riskTask = _riskAssessment.Get(id);
			await Task.WhenAll(loanTask, riskTask);

			return View(
				new LoanViewModel()
				{
					Loan = await loanTask,
					Risk = await riskTask
				}
			);
		}

		/// <summary>
		/// Used to get the list of loans to populate the index page.
		/// </summary>
		/// <param name="page"><see cref="Page"/> instance to get the list of loans in paginated format</param>
		/// <param name="filter"><see cref="Filter"/> instance to filter the list of loans</param>
		/// <returns></returns>
		[HttpGet("[action]")]
		public async Task<List<Loan>> List([FromQuery] Page page, [FromQuery] LoanFilter filter)
		{
			return await _loanManagement.GetAll(page, filter);
		}


		//TODO: remove this test action method
		[HttpPost("")]
		public async Task<IActionResult> Create()
		{
			if (await _loanManagement.Save(new Loan() { Id = 1007 }))
				return Ok();
			return StatusCode((int)HttpStatusCode.InternalServerError);
		}

		//TODO: remove this test action method
		[HttpGet("")]
		public async Task<IActionResult> Read([FromQuery] Page page, [FromQuery] LoanFilter filter)
		{
			return Ok(await _loanManagement.GetAll(page, filter));
		}

		//TODO: remove this test action method
		[HttpGet("{id}")]
		public async Task<IActionResult> ReadById(int id)
		{
			return Ok(await _loanManagement.Get(id));
		}

		//TODO: remove this test action method
		[HttpGet("risk/{id}")]
		public async Task<IActionResult> ReadRisk(int id)
		{
			return Ok(await _riskAssessment.Get(id));
		}

		//TODO: remove this test action method
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			if (await _loanManagement.Delete(id))
				return Ok();
			return StatusCode((int)HttpStatusCode.InternalServerError);
		}
	}
}
