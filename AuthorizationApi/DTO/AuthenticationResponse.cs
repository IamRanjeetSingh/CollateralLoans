using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationApi.DTO
{
	public class AuthenticationResponse
	{
		public bool IsSuccessful { get; set; }
		public string AccessToken { get; set; }
		public DateTime AccessTokenExpiresIn { get; set; }
		public string RefreshToken { get; set; }
		public DateTime RefreshTokenExpiresIn { get; set; }
	}
}
