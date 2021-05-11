using Microsoft.AspNetCore.Mvc;
using RiskAssessmentApi.Models;
using RiskAssessmentApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RiskAssessmentApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RiskController : ControllerBase
	{
		private IRiskAssessment _riskAssessment;

		public RiskController(IRiskAssessment riskAssessment)
		{
			_riskAssessment = riskAssessment;
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetByLoanId(int id)
		{
			Risk risk = await _riskAssessment.EvaluateAsync(id);
			if (risk == null)
				return BadRequest();
			return Ok(risk);
		}
	}
}
