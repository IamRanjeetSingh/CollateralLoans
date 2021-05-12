using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationApi.Services
{
	/// <summary>
	/// Responsible for managing access and refresh token to corresponding user ids.
	/// </summary>
	public interface ITokenManager
	{
		/// <summary>
		/// Save the tokens against the user id. If some tokens already exists for this id, they will be overwritten.
		/// </summary>
		/// <param name="userId">uniqe user id for which the tokens will be saved</param>
		/// <param name="accessToken">access token</param>
		/// <param name="refreshToken">refresh token</param>
		void SaveOrUpdateTokens(string userId, string accessToken, string refreshToken);

		/// <summary>
		/// Validate the tokens against the user id.
		/// </summary>
		/// <param name="userId">unique user id for which the tokens will be saved</param>
		/// <param name="accessToken">access token</param>
		/// <param name="refreshToken">refresh token</param>
		/// <returns></returns>
		bool ValidateTokens(string userId, string accessToken, string refreshToken);
	}
}
