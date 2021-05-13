using AuthorizationApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationApi.DAL.DAO
{
	public interface IUserTokensDao
	{

		/// <summary>
		/// Add new tokens or update existing tokens for the given user id.
		/// </summary>
		/// <param name="userTokens">wrapper around the user id and associated tokens</param>
		/// <returns>number of state entries written</returns>
		/// <exception cref="InvalidOperationException">more than one <see cref="UserTokens"/> found for the given user id</exception>
		int AddOrUpdate(UserTokens userTokens);

		/// <summary>
		/// Get a <see cref="UserTokens"/> instance associated with the given id.
		/// </summary>
		/// <param name="userId">unique user id for which the tokens has to be searched</param>
		/// <returns><see cref="UserTokens"/> instance for the given id or null if not found</returns>
		UserTokens GetByUserId(string userId);
	}
}
