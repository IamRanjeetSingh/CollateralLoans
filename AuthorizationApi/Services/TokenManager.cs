using AuthorizationApi.DAL.DAO;
using AuthorizationApi.DTO;
using AuthorizationApi.Models;
using Microsoft.Extensions.Logging;
using System;

namespace AuthorizationApi.Services
{
	/// <summary>
	/// Concrete implementation of <see cref="ITokenManager"/>
	/// </summary>
	public class TokenManager : ITokenManager
	{
		private ILogger<TokenManager> _logger;
		private IAccessTokenFactory _accessTokenFactory;
		private IRefreshTokenFactory _refreshTokenFactory;
		private IUserTokensDao _userTokensDao;

		public TokenManager(ILogger<TokenManager> logger, IAccessTokenFactory accessTokenFactory, IRefreshTokenFactory refreshTokenFactory, IUserTokensDao userTokensDao)
		{
			_logger = logger;
			_accessTokenFactory = accessTokenFactory;
			_refreshTokenFactory = refreshTokenFactory;
			_userTokensDao = userTokensDao;
		}

		public TokenContainer GenerateNewTokens(string userId)
		{
			Token accessToken = _accessTokenFactory.Generate();
			Token refreshToken = _refreshTokenFactory.Generate(userId);

			_userTokensDao.AddOrUpdate(new UserTokens()
			{
				UserId = userId,
				RefreshToken = refreshToken.AsString
			});

			return new TokenContainer(accessToken, refreshToken);
		}

		public TokenValidationResponse HandleValidationRequest(TokenValidationRequest request)
		{
			_logger.LogInformation("Handling validation request");//TODO: remove log
			if (request == null || string.IsNullOrEmpty(request.AccessToken) || (request.RefreshIfExpired && string.IsNullOrEmpty(request.RefreshToken)))
				throw new ArgumentException("invalid token validation request");

			_logger.LogInformation("arguments ok");//TODO: remove log
			if (_accessTokenFactory.Validate(request.AccessToken))
			{
				_logger.LogInformation("access token is valid");//TODO: remove log
				return new TokenValidationResponse()
				{
					IsValid = true,
					IsRefreshed = false,
					NewAccessToken = null,
					NewAccessTokenExpiresIn = null
				};
			}
			else if(request.RefreshIfExpired && _refreshTokenFactory.Validate(request.RefreshToken))
			{
				_logger.LogInformation("access token invalid but refresh token is valid");//TODO: remove log
				Token newAccessToken = _accessTokenFactory.Generate();
				return new TokenValidationResponse()
				{
					IsValid = true,
					IsRefreshed = true,
					NewAccessToken = newAccessToken.AsString,
					NewAccessTokenExpiresIn = newAccessToken.ExpiresIn
				};
			}
			else
			{
				_logger.LogInformation("neither access token nor refresh token is valid");//TODO: remove log
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
