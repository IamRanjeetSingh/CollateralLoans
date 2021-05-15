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
			return await Task.FromResult(true);
			//return await Task.FromResult(userId.ToLower().Trim() == "ranjeet" && password.ToLower().Trim() == "password");
		}
	}
}
