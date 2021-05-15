using AuthorizationApi.Exceptions;
using AuthorizationApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationApi.Services
{
	/// <summary>
	/// Responsible for generating new and validating old refresh tokens
	/// </summary>
	public interface IRefreshTokenFactory
	{
		/// <summary>
		/// Generate a new refresh token.
		/// </summary>
		/// <param name="userId">unique user id associated with the user to which this token belongs to</param>
		/// <returns><see cref="Token"/> containing the string refresh token and other info about it</returns>
		/// <exception cref="ArgumentNullException">userId is null</exception>
		Token Generate(string userId);

		/// <summary>
		/// Validate if this refresh token is valid and belongs to the user associated with the user id claim inside the token.
		/// </summary>
		/// <param name="userId">unique user id with which this token claims to be associated with</param>
		/// <param name="refreshToken">refresh token to be validated</param>
		/// <returns>true if this refresh token is valid and was issued for the user associated with the id inside the token, false otherwise</returns>
		/// <exception cref="ArgumentNullException">userId or refresh token is null</exception>
		bool Validate(string userId, string refreshToken);
	}
}
