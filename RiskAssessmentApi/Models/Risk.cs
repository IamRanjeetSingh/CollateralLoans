using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RiskAssessmentApi.Models
{
	public class Risk
	{
		public double LoanValue { get; set; }
		public double TotalCollateralValue { get; set; }
		public double Raw { get => LoanValue - TotalCollateralValue; }
		public double Percent { get => (LoanValue / TotalCollateralValue) * 100; }
		public DateTime LastAssessDate { get; set; }
	}
}
