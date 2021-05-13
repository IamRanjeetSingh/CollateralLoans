using AuthorizationApi.DAL.DAO;
using AuthorizationApi.Exceptions;
using AuthorizationApi.Models;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthorizationApi.Services
{
	/// <summary>
	/// Concrete implementation of <see cref="IAccessTokenFactory"/>
	/// </summary>
	public class AccessTokenFactory : IAccessTokenFactory
	{
		private readonly SecurityParameters _securityParams;
		private readonly int _expiresIn;
		private readonly IUserTokensDao _userTokensDao;

		/// <summary>
		/// Initialize a new instance of <see cref="AccessTokenFactory"/>
		/// </summary>
		/// <param name="securityParams">security parameters for the access token</param>
		/// <param name="expiresIn">validity of access token(in seconds)</param>
		public AccessTokenFactory(SecurityParameters securityParams, int expiresIn, IUserTokensDao userTokensDao)
		{
			_securityParams = securityParams;
			_expiresIn = expiresIn;
			_userTokensDao = userTokensDao;
		}

		public Token Generate()
		{
			JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
			DateTime expiresIn = DateTime.UtcNow.AddSeconds(_expiresIn);
			SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor()
			{
				Expires = expiresIn,
				SigningCredentials = new SigningCredentials(_securityParams.SigningKey, _securityParams.SecurityAlgorithmSignature)
			};

			SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);
			string tokenAsString = tokenHandler.WriteToken(securityToken);

			return new Token(tokenAsString, expiresIn);
		}

		public bool Validate(string accessToken)
		{
			JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
			try
			{
				tokenHandler.ValidateToken(
					accessToken, 
					new TokenValidationParameters() 
					{
						ValidateAudience = false,
						ValidateIssuer = false,
						ValidateIssuerSigningKey = true,
						ValidAlgorithms = new string[] { _securityParams.SecurityAlgorithm },
						IssuerSigningKey = _securityParams.SigningKey,
						ClockSkew = TimeSpan.Zero
					}, 
					out SecurityToken validatedToken);
				return true;
			}
			catch(SecurityTokenExpiredException) { return false; }
			catch (Exception) { throw new InvalidTokenException("invalid access token"); }
		}
	}
}
