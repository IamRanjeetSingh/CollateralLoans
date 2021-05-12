using AuthorizationApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationApi.Services
{
	/// <summary>
	/// Responsible for handling the access token generation.
	/// </summary>
	public interface IAccessTokenGenerator
	{
		/// <summary>
		/// Generate a new access token.
		/// </summary>
		/// <param name="userId">unique id of individual user which will be used to create the access token</param>
		/// <returns><see cref="Token"/> containing the string access token and other info about it</returns>
		Token Generate(string userId);
	}
}
