using AuthorizationApi.DTO;
using AuthorizationApi.Exceptions;
using AuthorizationApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationApi.Services
{
	/// <summary>
	/// Responsible for generating new access and refresh tokens and handling token validation requests.
	/// </summary>
	public interface ITokenManager
	{
		/// <summary>
		/// Handle the token validation request.
		/// </summary>
		/// <param name="request">validation request to be handledd</param>
		/// <returns><see cref="TokenValidationResponse"/> containing validation result</returns>
		/// <exception cref="ArgumentException">Request or UserId is null. The AccessToken can also be null if RefreshIfExpired property is false. The RefreshToken can also be null if RefreshIfExpired property is true</exception>
		/// <exception cref="InvalidTokenException">invalid token was provided in the request</exception>
		/// <exception cref="UnauthenticTokenException">refresh token was not issued for the claimed user</exception>
		TokenValidationResponse HandleValidationRequest(TokenValidationRequest request);

		/// <summary>
		/// Generate new tokens against the given user id. If old tokens exists in the system, they will be overwritten with the new ones.
		/// </summary>
		/// <param name="userId">unique user id against which the tokens will be generated</param>
		/// <returns><see cref="TokenContainer"/> containing both access token and refresh token</returns>
		TokenContainer GenerateNewTokens(string userId);
	}
}
