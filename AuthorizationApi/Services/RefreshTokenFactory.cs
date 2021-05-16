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
		private SecurityParameters _securityParams;
		private int _expiresIn;
		private IUserTokensDao _userTokensDao;

		public RefreshTokenFactory(SecurityParameters securityParams, int expiresIn, IUserTokensDao userTokensDao)
		{
			_securityParams = securityParams;
			_expiresIn = expiresIn;
			_userTokensDao = userTokensDao;
		}

		public Token Generate(string userId)
		{
			if (userId == null) 
				throw new ArgumentNullException("user id is null");

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

		public bool Validate(string userId, string refreshToken)
		{
			if (userId == null || refreshToken == null) throw new ArgumentNullException();

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

				if (claimsPrincipal == null || claimsPrincipal.Identity == null || claimsPrincipal.Identity.Name == null) 
					throw new UnauthenticTokenException("no claims found");

				UserTokens userTokens = _userTokensDao.GetByUserId(claimsPrincipal.Identity.Name);
				if (userTokens == null || userTokens.LastRefreshToken != refreshToken) 
					throw new UnauthenticTokenException("refresh tokens does not match for the given user id");

				if (validatedToken.ValidFrom >= validatedToken.ValidTo || validatedToken.ValidTo < DateTime.UtcNow)
					throw new SecurityTokenExpiredException("token lifetime is invalid");

				return true;
			}
			catch (Exception) { return false; }
		}
	}
}
