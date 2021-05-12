using AuthorizationApi.DTO;
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
	public class TokenValidationHandler : ITokenValidationHandler
	{
		private SecurityParameters _securityParams;
		private ITokenManager _tokenManager;

		public TokenValidationHandler(SecurityParameters securityParams, ITokenManager tokenManager)
		{
			_securityParams = securityParams;
			_tokenManager = tokenManager;
		}

		public TokenValidationResponse Validate(TokenValidationRequest request)
		{
			JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
			ClaimsPrincipal claimsPrincipal;
			SecurityToken validatedToken;
			bool isTokenValid = true;
			try
			{
				claimsPrincipal = tokenHandler.ValidateToken(
					request.AccessToken,
					new TokenValidationParameters()
					{
						ValidateIssuer = false,
						ValidateAudience = false,
						ValidateIssuerSigningKey = true,
						IssuerSigningKey = _securityParams.SigningKey
					}, out validatedToken);
			}
			catch(SecurityTokenExpiredException)
			{
				
			}
			catch(Exception) { isTokenValid = false; }

			JwtSecurityToken jwtToken = validatedToken as JwtSecurityToken;

			if (isTokenValid && (jwtToken == null || jwtToken.Header.Alg != _securityParams.SecurityAlgorithm || claimsPrincipal.Identity.Name != request.UserId))
				isTokenValid = false;

			if (isTokenValid)
				return new TokenValidationResponse()
				{

				};
			else
				return new TokenValidationResponse()
				{
					AccessToken = "",
					AccessTokenExpiresIn = DateTime.MinValue,
					IsValid = false,
					RefreshIfExpired = request.RefreshIfExpired,
					RefreshToken = request.RefreshToken,
					UserId = request.UserId
				};
		}
	}
}
