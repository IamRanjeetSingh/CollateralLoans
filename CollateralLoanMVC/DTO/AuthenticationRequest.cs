using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollateralLoanMVC.DTO
{
	public class AuthenticationRequest
	{
		public string UserId { get; set; }
		public string Password { get; set; }
	}
}
