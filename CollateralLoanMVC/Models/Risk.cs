using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollateralLoanMVC.Models
{
	public class Risk
	{
		public double LoanValue { get; set; }
		public double TotalCollateralValue { get; set; }
		public double Raw { get => Math.Max(LoanValue - TotalCollateralValue, 0); }
		public double Percent { get => LoanValue != 0 ? (Raw * 100) / LoanValue : 0; }
		public DateTime LastAssessDate { get; set; }
	}
}
