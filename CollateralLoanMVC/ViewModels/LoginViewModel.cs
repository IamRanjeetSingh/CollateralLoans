using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CollateralLoanMVC.ViewModels
{
	public class LoginViewModel
	{
		[Required(ErrorMessage = "required")]
		public string UserId { get; set; }
		[Required(ErrorMessage = "required")]
		public string Password { get; set; }
	}
}
