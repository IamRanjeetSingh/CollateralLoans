using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollateralLoanMVC.Models
{
	public class Loan
	{
		public int Id { get; set; }
		public int CustomerId { get; set; }
		public string Type { get; set; }
		public double Principal { get; set; }
		public double Interest { get; set; }
		public DateTime SanctionDate { get; set; }
		public DateTime MaturityDate { get; set; }
		//public TimeSpan Tenure { get => MaturityDate - SanctionDate; }//TODO: remove this
		public double Emi { get; set; }
	}
}
