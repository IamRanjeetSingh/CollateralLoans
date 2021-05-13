using AuthorizationApi.Exceptions;
using AuthorizationApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationApi.Services
{
	/// <summary>
	/// Responsible for generating new and validating old access tokens
	/// </summary>
	public interface IAccessTokenFactory
	{
		/// <summary>
		/// Generate a new access token.
		/// </summary>
		/// <returns><see cref="Token"/> containing the string access token and other info about it</returns>
		Token Generate();

		/// <summary>
		/// Validate if this access token is valid
		/// </summary>
		/// <param name="accessToken">access token to be validated</param>
		/// <returns>true if this access token is valid, false otherwise</returns>
		/// <exception cref="InvalidTokenException">token is invalid</exception>
		bool Validate(string accessToken);
	}
}
