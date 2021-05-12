using AuthorizationApi.Models;
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
	/// Concrete implementation of <see cref="IRefreshTokenGenerator"/>
	/// </summary>
	public class RefreshTokenGenerator : IRefreshTokenGenerator
	{
		private SecurityParameters _securityParams;
		private int _expiresIn;

		public RefreshTokenGenerator(SecurityParameters securityParams, int expiresIn)
		{
			_securityParams = securityParams;
			_expiresIn = expiresIn;
		}

		public Token Generate(string userId)
		{
			JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
			DateTime expiresIn = DateTime.UtcNow.AddSeconds(_expiresIn);
			SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor()
			{
				Subject = new ClaimsIdentity(new Claim[] { new Claim("userId", userId) }),
				Expires = expiresIn,
				SigningCredentials = new SigningCredentials(_securityParams.SigningKey, _securityParams.SecurityAlgorithmSignature)
			};

			SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);
			string tokenAsString = tokenHandler.WriteToken(securityToken);

			return new Token(tokenAsString, expiresIn);
		}
	}
}
