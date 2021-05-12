using AuthorizationApi.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationApi.Services
{
	/// <summary>
	/// Service for handling user details
	/// </summary>
	public interface IUserManager
	{
		/// <summary>
		/// Validate the provided user credentials
		/// </summary>
		/// <param name="userId">unique id for individual user</param>
		/// <param name="password">password associated with the user</param>
		/// <returns>true if the credentials are valid, false otherwise</returns>
		Task<bool> ValidateCredentials(string userId, string password);
	}
}
