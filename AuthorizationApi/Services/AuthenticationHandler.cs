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
		private IAccessTokenGenerator _accessTokenGenerator;
		private IRefreshTokenGenerator _refreshTokenGenerator;

		public AuthenticationHandler(IUserManager userManager, IAccessTokenGenerator accessTokenGenerator, IRefreshTokenGenerator refreshTokenGenerator)
		{
			_userManager = userManager;
			_accessTokenGenerator = accessTokenGenerator;
			_refreshTokenGenerator = refreshTokenGenerator;
		}

		public async Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request)
		{
			if (!await _userManager.ValidateCredentials(request.UserId, request.Password))
			{
				return new AuthenticationResponse()
				{
					IsSuccessful = false,
					AccessToken = null,
					AccessTokenExpiresIn = DateTime.MinValue,
					RefreshToken = null,
					RefreshTokenExpiresIn = DateTime.MinValue
				};
			}

			Token accessToken = _accessTokenGenerator.Generate(request.UserId);
			Token refreshToken = _refreshTokenGenerator.Generate(request.UserId);

			return new AuthenticationResponse()
			{
				AccessToken = accessToken.AsString,
				AccessTokenExpiresIn = accessToken.ExpiresIn,
				RefreshToken = refreshToken.AsString,
				RefreshTokenExpiresIn = refreshToken.ExpiresIn
			};
		}
	}
}
