using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollateralLoanMVC.DTO
{
	public class TokenValidationResponseBody
	{
		public bool IsValid { get; set; }
		public string NewAccessToken { get; set; }
		public DateTime? NewAccessTokenExpiresIn { get; set; }
		public bool IsRefreshed { get; set; }
	}
}
