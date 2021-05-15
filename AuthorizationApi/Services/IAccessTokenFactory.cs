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
		/// <param name="userId">unique user id associated with the user to which this token belongs to</param>
		/// <returns><see cref="Token"/> containing the string access token and other info about it</returns>
		/// <exception cref="ArgumentNullException">userId is null</exception>
		Token Generate(string userId);

		/// <summary>
		/// Validate if this access token is valid and it belongs to the user it claims to.
		/// </summary>
		/// <param name="userId">unique user id with which this token claims to be associated with</param>
		/// <param name="accessToken">access token to be validated</param>
		/// <returns>true if this access token is valid, false otherwise</returns>
		/// <exception cref="ArgumentNullException">userId or accessToken is null</exception>
		bool Validate(string userId, string accessToken);
	}
}
