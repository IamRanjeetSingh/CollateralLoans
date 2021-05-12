using AuthorizationApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationApi.Services
{
	/// <summary>
	/// Responsible for handling the refresh token generation.
	/// </summary>
	public interface IRefreshTokenGenerator
	{
		/// <summary>
		/// Generate a new refresh token.
		/// </summary>
		/// <param name="userId">unique id of individual user which will be used to create the access token</param>
		/// <returns><see cref="Token"/> containing the string refresh token and other info about it</returns>
		Token Generate(string userId);
	}
}
