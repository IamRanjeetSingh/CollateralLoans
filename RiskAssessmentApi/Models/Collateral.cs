using System;

namespace RiskAssessmentApi.Models
{
	public class Collateral
	{
		public int Id { get; set; }
		public int LoanId { get; set; }
		public int CustomerId { get; set; }
		public string Type { get; protected set; }
		public DateTime InitialAssesDate { get; set; }
		public DateTime LastAssessDate { get; set; }
		public double InitialValue { get; set; }
		public double CurrentValue { get; set; }

		public Collateral()
		{ }
	}
}
