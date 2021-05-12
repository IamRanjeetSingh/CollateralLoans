using AuthorizationApi.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationApi.Services
{
	/// <summary>
	/// Responsible for authenticating users and generating tokens for them.
	/// </summary>
	public interface IAuthenticationHandler
	{
		/// <summary>
		/// Authenticate the users based on the authentication request
		/// </summary>
		/// <param name="request">request containing necessary information for authentication</param>
		/// <returns><see cref="AuthenticationResponse"/> representing the authentication result</returns>
		Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request);
	}
}
