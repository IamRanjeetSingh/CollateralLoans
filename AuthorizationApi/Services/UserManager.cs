using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace AuthorizationApi.Services
{
	/// <summary>
	/// Concrete implementation of <see cref="UserManager"/>
	/// </summary>
	public class UserManager : IUserManager
	{
		private ILogger<UserManager> _logger;

		public UserManager(ILogger<UserManager> logger)
		{
			_logger = logger;
		}

		public async Task<bool> ValidateCredentials(string userId, string password)
		{
			_logger.LogInformation(userId+" "+password);

			//TODO: add actual user credentials validation logic
			//return await Task.FromResult(true);

			if (userId.ToLower().Trim() == "ranjeet" && password.ToLower().Trim() == "password")
				return await Task.FromResult(true);
			else if (userId.ToLower().Trim() == "mirazul" && password.ToLower().Trim() == "password")
				return await Task.FromResult(true);
			else if (userId.ToLower().Trim() == "anushree" && password.ToLower().Trim() == "password")
				return await Task.FromResult(true);
			else if (userId.ToLower().Trim() == "harika" && password.ToLower().Trim() == "password")
				return await Task.FromResult(true);
			else if (userId.ToLower().Trim() == "sai" && password.ToLower().Trim() == "password")
				return await Task.FromResult(true);
			else
				return await Task.FromResult(false);

			//return await Task.FromResult(userId.ToLower().Trim() == "ranjeet" && password.ToLower().Trim() == "password");
		}
	}
}
