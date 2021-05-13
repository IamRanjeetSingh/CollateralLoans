using AuthorizationApi.DTO;
using AuthorizationApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

		public AuthenticationHandler(IUserManager userManager, ITokenManager tokenManager)
		{
			_userManager = userManager;
			_tokenManager = tokenManager;
		}

		public async Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request)
		{
			if (request == null || string.IsNullOrEmpty(request.UserId) || string.IsNullOrEmpty(request.Password))
				throw new ArgumentException("invalid authentication request");

			if (!await _userManager.ValidateCredentials(request.UserId, request.Password))
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
