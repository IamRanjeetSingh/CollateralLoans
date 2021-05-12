using Microsoft.AspNetCore.Mvc;
using RiskAssessmentApi.Exceptions;
using RiskAssessmentApi.Models;
using RiskAssessmentApi.Services;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace RiskAssessmentApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RiskController : ControllerBase
	{
		/// <summary>
		/// Evaluate risk for a particular loan using this service
		/// </summary>
		private IRiskAssessment _riskAssessment;

		public RiskController(IRiskAssessment riskAssessment)
		{
			_riskAssessment = riskAssessment;
		}

		/// <summary>
		/// Get a <see cref="Risk"/> instance for the loan associated with the given id.
		/// </summary>
		/// <param name="id">id of the loan for which the risk details will be returned</param>
		/// <returns><see cref="Risk"/> instance for the requested loan</returns>
		/// <response code="200"><see cref="Risk"/> instance for the requested loan</response>
		/// <response code="404">no loan or collaterals found for the given loan id</response>
		/// <response code="503">either LoanManagementApi or CollateralManagementApi is unavailable</response>
		/// <response code="500">something went wrong in either LoanManagementApi or CollateralManagementApi</response>
		[HttpGet("{id}")]
		public async Task<IActionResult> GetByLoanId(int id)
		{
			Risk risk;

			try { risk = await _riskAssessment.EvaluateAsync(id); }
			catch (HttpRequestException) { return StatusCode((int)HttpStatusCode.ServiceUnavailable, new { error = "either LoanManagementApi or CollateralManagementApi is unavailable" }); }
			catch (UnexpectedResponseException) { return StatusCode((int)HttpStatusCode.InternalServerError, new { error = "something went wrong in either LoanManagementApi or CollateralManagementApi" }); }

			if (risk == null)
				return NotFound(new { error = "no loan or collaterals found for the given loan id" });

			return Ok(risk);
		}
	}
}
