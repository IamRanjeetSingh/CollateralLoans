using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationApi.DTO
{
	public class TokenValidationResponse
	{
		public bool IsValid { get; set; }
		public string NewAccessToken { get; set; }
		public DateTime? NewAccessTokenExpiresIn { get; set; }
		public bool IsRefreshed { get; set; }
	}
}
