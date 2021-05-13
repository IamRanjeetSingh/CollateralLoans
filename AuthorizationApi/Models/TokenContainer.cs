using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationApi.Models
{
	public class TokenContainer
	{
		public Token AccessToken { get; }
		public Token RefreshToken { get; }

		public TokenContainer(Token accessToken, Token refreshToken)
		{
			AccessToken = accessToken;
			RefreshToken = refreshToken;
		}
	}
}
