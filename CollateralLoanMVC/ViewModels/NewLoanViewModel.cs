﻿using CollateralLoanMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollateralLoanMVC.ViewModels
{
	public class NewLoanViewModel
	{
		public Loan Loan { get; set; }
		public Collateral Collateral { get; set; }
	}
}
