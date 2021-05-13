using AuthorizationApi.DTO;
using AuthorizationApi.Exceptions;
using AuthorizationApi.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AuthorizationApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TokenController : ControllerBase
	{
		private ITokenManager _tokenManager;

		public TokenController(ITokenManager tokenManager)
		{
			_tokenManager = tokenManager;
		}

		/// <summary>
		/// Validate an access token
		/// </summary>
		/// <param name="request">wrapper around necessary information for token validation</param>
		/// <returns><see cref="TokenValidationResponse"/> instance containing the token validation result</returns>
		/// <response code="200">token validation request is processed successfully</response>
		/// <response code="400">invalid token validation request</response>
		[HttpPost("")]
		public IActionResult Validate([FromBody] TokenValidationRequest request)
		{
			try { return Ok(_tokenManager.HandleValidationRequest(request)); }
			catch (Exception e) when (e is ArgumentException || e is InvalidTokenException || e is UnauthenticTokenException)
			{ return BadRequest(new { error = "invalid token validation request" }); }
		}
	}
}
