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
	/// Concrete implementation of <see cref="IAccessTokenGenerator"/>
	/// </summary>
	public class AccessTokenGenerator : IAccessTokenGenerator
	{
		private readonly SecurityParameters _securityParams;
		private readonly int _expiresIn;

		/// <summary>
		/// Initialize a new instance of <see cref="AccessTokenGenerator"/>
		/// </summary>
		/// <param name="securityParams">security parameters for the access token</param>
		/// <param name="expiresIn">validity of access token(in seconds)</param>
		public AccessTokenGenerator(SecurityParameters securityParams, int expiresIn)
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
				Subject = new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, userId) }),
				Expires = expiresIn,
				SigningCredentials = new SigningCredentials(_securityParams.SigningKey, _securityParams.SecurityAlgorithmSignature)
			};

			SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);
			string tokenAsString = tokenHandler.WriteToken(securityToken);

			return new Token(tokenAsString, expiresIn);
		}
	}
}
