using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationApi.Services
{
	/// <summary>
	/// Concrete implementation of <see cref="UserManager"/>
	/// </summary>
	public class UserManager : IUserManager
	{
		public async Task<bool> ValidateCredentials(string userId, string password)
		{
			//TODO: add actual user credentials validation logic
			return await Task.FromResult(userId == "ranjeet" && password == "password");
		}
	}
}
