using AuthorizationApi.DTO;
using AuthorizationApi.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationApi.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class TokenController : ControllerBase
	{
		private ITokenValidationHandler _tokenValidationHandler;

		public TokenController(ITokenValidationHandler tokenValidationHandler)
		{
			_tokenValidationHandler = tokenValidationHandler;
		}

		/// <summary>
		/// Validate an access token
		/// </summary>
		/// <param name="request">wrapper around necessary information for token validation</param>
		/// <returns><see cref="TokenValidationResponse"/> instance containing the token validation result</returns>
		/// <response code="200">token validation request is processed successfully</response>
		[HttpPost("[action]")]
		public IActionResult Validate(TokenValidationRequest request)
		{
			return Ok(_tokenValidationHandler.Validate(request));
		}
	}
}
