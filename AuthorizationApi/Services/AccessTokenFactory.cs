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
		private readonly IUserTokensDao _userTokensDao;//TODO: remove this if unused

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

		public Token Generate(string userId)
		{
			if (userId == null) throw new ArgumentNullException();

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

		public bool Validate(string userId, string accessToken)
		{
			if (userId == null || accessToken == null) throw new ArgumentNullException();

			JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
			try
			{
				ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(
					accessToken, 
					new TokenValidationParameters() 
					{
						ValidateAudience = false,
						ValidateIssuer = false,
						ValidateLifetime = false,
						ValidateIssuerSigningKey = true,
						ValidAlgorithms = new string[] { _securityParams.SecurityAlgorithm },
						IssuerSigningKey = _securityParams.SigningKey,
						ClockSkew = TimeSpan.Zero
					}, 
					out SecurityToken validatedToken);

				if (
					claimsPrincipal == null ||
					claimsPrincipal.Identity == null ||
					claimsPrincipal.Identity.Name == null ||
					claimsPrincipal.Identity.Name != userId)
					throw new UnauthenticTokenException("token does not belong to the user it claims to");

				if (validatedToken.ValidFrom >= validatedToken.ValidTo || validatedToken.ValidTo < DateTime.UtcNow)
					throw new SecurityTokenExpiredException("token lifetime is invalid");

				return true;
			}
			catch (Exception) { return false; }
		}
	}
}
