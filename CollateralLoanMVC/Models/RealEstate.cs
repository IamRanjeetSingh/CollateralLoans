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
		public double InitialLandPricePerSqFt { get; set; }
		public double CurrentLandPricePerSqFt { get; set; }
		public double InitialStructurePrice { get; set; }
		public double CurrentStructurePrice { get; set; }

		public override double InitialValue 
		{
			get
			{
				return (AreaInSqFt * InitialLandPricePerSqFt) + InitialStructurePrice;
			}
		}

		public override double CurrentValue
		{
			get
			{
				return (AreaInSqFt * CurrentLandPricePerSqFt) + CurrentStructurePrice;
			}
		}

		public RealEstate()
		{
			Type = CollateralType.RealEstate;
		}
	}
}
