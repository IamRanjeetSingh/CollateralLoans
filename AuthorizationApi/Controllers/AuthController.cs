using AuthorizationApi.DTO;
using AuthorizationApi.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationApi.Controllers
{
	/// <summary>
	/// Responsible for authenticating user credentials and generating jwt token for them.
	/// </summary>
	[Route("[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		/// <summary>
		/// Use the service for authenticating the users and generating tokens for them.
		/// </summary>
		private IAuthenticationHandler _authHandler;

		public AuthController(IAuthenticationHandler authHandler)
		{
			_authHandler = authHandler;
		}

		/// <summary>
		/// Authenticate the user based on the given credentials
		/// </summary>
		/// <param name="request">wrapper around necessary information for authentication</param>
		/// <returns><see cref="AuthenticationResponse"/> instance containing the authentication result</returns>
		/// <response code="200">authentication request is successfully processed</response>
		[HttpPost("")]
		public async Task<IActionResult> Authenticate([FromBody] AuthenticationRequest request)
		{
			return Ok(await _authHandler.AuthenticateAsync(request));
		}
	}
}
