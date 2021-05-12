using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationApi.DTO
{
	public class TokenValidationResponse
	{
		public bool IsValid { get; set; }
		public string UserId { get; set; }
		public string AccessToken { get; set; }
		public DateTime AccessTokenExpiresIn { get; set; }
		public bool RefreshIfExpired { get; set; }
		public string RefreshToken { get; set; }

	}
}
