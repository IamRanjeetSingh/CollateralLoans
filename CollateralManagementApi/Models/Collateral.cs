using System;

namespace CollateralManagementApi.Models
{
	public class Collateral
	{
		public int Id { get; set; }
		public int LoanId { get; set; }
		public int CustomerId { get; set; }
		public string Type { get; protected set; }
		public DateTime InitialAssesDate { get; set; }
		public DateTime LastAssessDate { get; set; }
		public virtual double InitialValue { get; }
		public virtual double CurrentValue { get; }

		public Collateral()
		{ }
	}
}
