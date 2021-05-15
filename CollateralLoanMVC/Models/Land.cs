using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollateralLoanMVC.Models
{
	public class Land : Collateral
	{
		public double AreaInSqFt { get; set; }
		public double InitialPricePerSqFt { get; set; }
		public double CurrentPricePerSqFt { get; set; }

		public override double InitialValue 
		{ 
			get
			{
				return InitialPricePerSqFt * AreaInSqFt;
			}
		}
		public override double CurrentValue 
		{ 
			get
			{
				return CurrentPricePerSqFt * AreaInSqFt;
			}
		}

		public Land()
		{
			Type = CollateralType.Land;
		}
	}
}
