using AuthorizationApi.DTO;
using AuthorizationApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace AuthorizationApi.Controllers
{
	/// <summary>
	/// Responsible for authenticating user credentials and generating jwt token for them.
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		/// <summary>
		/// Use the service for authenticating the users and generating tokens for them.
		/// </summary>
		private IAuthenticationHandler _authHandler;
		private ILogger<AuthController> _logger;

		public AuthController(IAuthenticationHandler authHandler, ILogger<AuthController> logger)
		{
			_authHandler = authHandler;
			_logger = logger;
		}

		/// <summary>
		/// Authenticate the user based on the given credentials
		/// </summary>
		/// <param name="request">wrapper around necessary information for authentication</param>
		/// <returns><see cref="AuthenticationResponse"/> instance containing the authentication result</returns>
		/// <response code="200">authentication request is successfully processed</response>
		/// <response code="400">authentication request is invalid, either request is null or userid or password is null or empty</response>
		[HttpPost("")]
		public async Task<IActionResult> Authenticate([FromBody] AuthenticationRequest request)
		{
			_logger.LogInformation(JsonSerializer.Serialize(request));
			try 
			{
				AuthenticationResponse response = await _authHandler.AuthenticateAsync(request);
				_logger.LogInformation(JsonSerializer.Serialize(response));
				return Ok(response); 
			}
			catch (ArgumentException) { return BadRequest(new { error = "invalid authentication request" }); }
		}
	}
}
