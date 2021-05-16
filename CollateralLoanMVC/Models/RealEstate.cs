using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollateralLoanMVC.Models
{
	public class RealEstate : Collateral
	{
		public int YearBuilt { get; set; }
		public double AreaInSqFt { get; set; }
		public double InitialLandPriceInSqFt { get; set; }
		public double CurrentLandPriceInSqFt { get; set; }
		public double InitialStructurePrice { get; set; }
		public double CurrentStructurePrice { get; set; }

		public override double InitialValue 
		{
			get
			{
				return (AreaInSqFt * InitialLandPriceInSqFt) + InitialStructurePrice;
			}
		}

		public override double CurrentValue
		{
			get
			{
				return (AreaInSqFt * CurrentLandPriceInSqFt) + CurrentStructurePrice;
			}
		}

		public RealEstate()
		{
			Type = CollateralType.RealEstate;
		}
	}
}
