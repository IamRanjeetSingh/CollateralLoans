using AuthorizationApi.DTO;
using AuthorizationApi.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace AuthorizationApi.Services
{
	/// <summary>
	/// Concrete implementation of <see cref="IAuthenticationHandler"/>
	/// </summary>
	public class AuthenticationHandler : IAuthenticationHandler
	{
		private IUserManager _userManager;
		private ITokenManager _tokenManager;
		private ILogger<AuthenticationHandler> _logger;

		public AuthenticationHandler(IUserManager userManager, ITokenManager tokenManager, ILogger<AuthenticationHandler> logger)
		{
			_userManager = userManager;
			_tokenManager = tokenManager;
			_logger = logger;
		}

		public async Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request)
		{
			if (request == null)
				throw new ArgumentNullException("request is null");
			if (string.IsNullOrEmpty(request.UserId) || string.IsNullOrEmpty(request.Password))
				throw new ArgumentException("invalid authentication request");

			if(_logger != null)
				_logger.LogInformation(JsonSerializer.Serialize(request));

			bool validationResult = await _userManager.ValidateCredentials(request.UserId, request.Password);

			if (_logger != null)
				_logger.LogInformation("credentials validation result: "+validationResult);

			if (!validationResult)
			{
				return new AuthenticationResponse()
				{
					IsSuccessful = false,
					AccessToken = null,
					AccessTokenExpiresIn = null,
					RefreshToken = null,
					RefreshTokenExpiresIn = null
				};
			}

			TokenContainer tokenContainer = _tokenManager.GenerateNewTokens(request.UserId);

			return new AuthenticationResponse()
			{
				IsSuccessful = true,
				AccessToken = tokenContainer.AccessToken.AsString,
				AccessTokenExpiresIn = tokenContainer.AccessToken.ExpiresIn,
				RefreshToken = tokenContainer.RefreshToken.AsString,
				RefreshTokenExpiresIn = tokenContainer.RefreshToken.ExpiresIn
			};
		}
	}
}
