using AuthorizationApi.DAL.DAO;
using AuthorizationApi.Exceptions;
using AuthorizationApi.Models;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AuthorizationApi.Services
{
	/// <summary>
	/// Concrete implementation of <see cref="IRefreshTokenFactory"/>
	/// </summary>
	public class RefreshTokenFactory : IRefreshTokenFactory
	{
		private ILogger<RefreshTokenFactory> _logger;
		private SecurityParameters _securityParams;
		private int _expiresIn;
		private IUserTokensDao _userTokensDao;

		public RefreshTokenFactory(ILogger<RefreshTokenFactory> logger, SecurityParameters securityParams, int expiresIn, IUserTokensDao userTokensDao)
		{
			_logger = logger;
			_securityParams = securityParams;
			_expiresIn = expiresIn;
			_userTokensDao = userTokensDao;
		}

		public Token Generate(string userId)
		{
			JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
			DateTime expiresIn = DateTime.UtcNow.AddSeconds(_expiresIn);
			SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor()
			{
				Subject = new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, userId) }),
				Expires = expiresIn,
				SigningCredentials = new SigningCredentials(_securityParams.SigningKey, _securityParams.SecurityAlgorithmSignature)
			};

			SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);
			string tokenAsString = tokenHandler.WriteToken(securityToken);

			return new Token(tokenAsString, expiresIn);
		}

		public bool Validate(string refreshToken)
		{
			_logger.LogInformation($"Validating Refresh token for {refreshToken}");//TODO: remove log
			JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
			try
			{
				ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(
					refreshToken,
					new TokenValidationParameters()
					{
						ValidateAudience = false,
						ValidateIssuer = false,
						ValidateLifetime = false,
						ValidateIssuerSigningKey = true,
						IssuerSigningKey = _securityParams.SigningKey,
						ValidAlgorithms = new string[] { _securityParams.SecurityAlgorithm }
					},
					out SecurityToken validatedToken);

				if (claimsPrincipal == null || claimsPrincipal.Identity == null || claimsPrincipal.Identity.Name == null) throw new UnauthenticTokenException("no claims found");
				UserTokens userTokens = _userTokensDao.GetByUserId(claimsPrincipal.Identity.Name);
				if (userTokens == null || userTokens.RefreshToken != refreshToken) throw new UnauthenticTokenException("refresh tokens does not match for the given user id");

				_logger.LogInformation($"{validatedToken.ValidFrom} < {validatedToken.ValidTo}: {validatedToken.ValidFrom < validatedToken.ValidTo}");//TODO: remove log
				_logger.LogInformation($"{validatedToken.ValidTo} >= {DateTime.UtcNow}: {validatedToken.ValidTo >= DateTime.UtcNow}");//TODO: remove log

				return validatedToken.ValidFrom < validatedToken.ValidTo && validatedToken.ValidTo >= DateTime.UtcNow;
			}
			catch (UnauthenticTokenException) { throw; }
			catch (Exception) { throw new InvalidTokenException("invalid refresh token"); }
		}
	}
}
