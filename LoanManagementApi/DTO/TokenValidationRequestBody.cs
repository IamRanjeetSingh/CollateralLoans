using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoanManagementApi.DTO
{
	public class TokenValidationRequestBody
	{
		public string UserId { get; set; }
		public string AccessToken { get; set; }
		public bool RefreshIfExpired { get; set; }
		public string RefreshToken { get; set; }
	}
}
