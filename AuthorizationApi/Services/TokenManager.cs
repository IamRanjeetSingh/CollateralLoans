using AuthorizationApi.DAL.DAO;
using AuthorizationApi.DTO;
using AuthorizationApi.Models;
using System;

namespace AuthorizationApi.Services
{
	/// <summary>
	/// Concrete implementation of <see cref="ITokenManager"/>
	/// </summary>
	public class TokenManager : ITokenManager
	{
		private IAccessTokenFactory _accessTokenFactory;
		private IRefreshTokenFactory _refreshTokenFactory;
		private IUserTokensDao _userTokensDao;

		public TokenManager(IAccessTokenFactory accessTokenFactory, IRefreshTokenFactory refreshTokenFactory, IUserTokensDao userTokensDao)
		{
			_accessTokenFactory = accessTokenFactory;
			_refreshTokenFactory = refreshTokenFactory;
			_userTokensDao = userTokensDao;
		}

		public TokenContainer GenerateNewTokens(string userId)
		{
			Token accessToken = _accessTokenFactory.Generate(userId);
			Token refreshToken = _refreshTokenFactory.Generate(userId);

			_userTokensDao.AddOrUpdate(new UserTokens()
			{
				UserId = userId,
				LastRefreshToken = refreshToken.AsString
			});

			return new TokenContainer(accessToken, refreshToken);
		}

		public TokenValidationResponse HandleValidationRequest(TokenValidationRequest request)
		{
			if (request == null || request.UserId == null || string.IsNullOrEmpty(request.AccessToken) || (request.RefreshIfExpired && string.IsNullOrEmpty(request.RefreshToken)))
				throw new ArgumentException("invalid token validation request");

			if (_accessTokenFactory.Validate(request.UserId, request.AccessToken))
			{
				return new TokenValidationResponse()
				{
					IsValid = true,
					IsRefreshed = false,
					NewAccessToken = null,
					NewAccessTokenExpiresIn = null
				};
			}
			else if (_refreshTokenFactory.Validate(request.UserId, request.RefreshToken))
			{
				Token token = _accessTokenFactory.Generate(request.UserId);
				return new TokenValidationResponse()
				{
					IsValid = true,
					IsRefreshed = true,
					NewAccessToken = token.AsString,
					NewAccessTokenExpiresIn = token.ExpiresIn
				};
			}
			else
			{
				return new TokenValidationResponse()
				{
					IsValid = false,
					IsRefreshed = false,
					NewAccessToken = null,
					NewAccessTokenExpiresIn = null
				};
			}
		}
	}
}