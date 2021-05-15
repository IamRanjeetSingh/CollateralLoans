using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CollateralLoanMVC.ViewModels
{
	public class SignupViewModel
	{
		[Required]
		public string Name { get; set; }
		[Required]
		public string UserId { get; set; }
		[Required]
		public string Password { get; set; }
	}
}
