using CollateralLoanMVC.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace CollateralLoanMVC.Util
{
	public class FormReader
	{
		public static Loan GetLoan(IFormCollection form)
		{
			return new Loan()
			{
				Id = int.Parse(form[$"{nameof(Loan)}_{nameof(Loan.Id)}"].ToString().Trim()),
				CustomerId = int.Parse(form[$"{nameof(Loan)}_{nameof(Loan.CustomerId)}"].ToString().Trim()),
				Type = form[$"{nameof(Loan)}_{nameof(Loan.Type)}"],
				Principal = double.Parse(form[$"{nameof(Loan)}_{nameof(Loan.Principal)}"].ToString().Trim()),
				Interest = double.Parse(form[$"{nameof(Loan)}_{nameof(Loan.Interest)}"].ToString().Trim()),
				Emi = double.Parse(form[$"{nameof(Loan)}_{nameof(Loan.Emi)}"]),
				SanctionDate = DateTime.Parse(form[$"{nameof(Loan)}_{nameof(Loan.SanctionDate)}"]),
				MaturityDate = DateTime.Parse(form[$"{nameof(Loan)}_{nameof(Loan.MaturityDate)}"])
			};
		}

		public static Collateral GetCollateral(IFormCollection form)
		{
			if (form[$"{nameof(Collateral)}_{nameof(Collateral.Type)}"] == CollateralType.Land)
			{
				return new Land()
				{
					Id = int.Parse(form[$"{nameof(Collateral)}_{nameof(Collateral.Id)}"].ToString().Trim()),
					LoanId = int.Parse(form[$"{nameof(Loan)}_{nameof(Loan.Id)}"].ToString().Trim()),
					CustomerId = int.Parse(form[$"{nameof(Loan)}_{nameof(Loan.CustomerId)}"].ToString().Trim()),
					InitialAssesDate = DateTime.Parse(form[$"{nameof(Collateral)}_{nameof(Collateral.InitialAssesDate)}"]),
					LastAssessDate = DateTime.Parse(form[$"{nameof(Collateral)}_{nameof(Collateral.LastAssessDate)}"]),
					AreaInSqFt = double.Parse(form[$"{nameof(Collateral)}_{nameof(Land)}_{nameof(Land.AreaInSqFt)}"]),
					InitialPricePerSqFt = double.Parse(form[$"{nameof(Collateral)}_{nameof(Land)}_{nameof(Land.InitialPricePerSqFt)}"]),
					CurrentPricePerSqFt = double.Parse(form[$"{nameof(Collateral)}_{nameof(Land)}_{nameof(Land.CurrentPricePerSqFt)}"])
				};
			}
			else if (form["collateral_type"] == CollateralType.RealEstate)
			{
				return new RealEstate()
				{
					Id = int.Parse(form[$"{nameof(Collateral)}_{nameof(Collateral.Id)}"].ToString().Trim()),
					LoanId = int.Parse(form[$"{nameof(Loan)}_{nameof(Loan.Id)}"].ToString().Trim()),
					CustomerId = int.Parse(form[$"{nameof(Loan)}_{nameof(Loan.CustomerId)}"].ToString().Trim()),
					InitialAssesDate = DateTime.Parse(form[$"{nameof(Collateral)}_{nameof(Collateral.InitialAssesDate)}"]),
					LastAssessDate = DateTime.Parse(form[$"{nameof(Collateral)}_{nameof(Collateral.LastAssessDate)}"]),
					AreaInSqFt = double.Parse(form[$"{nameof(Collateral)}_{nameof(RealEstate)}_{nameof(RealEstate.AreaInSqFt)}"]),
					InitialLandPricePerSqFt = double.Parse(form[$"{nameof(Collateral)}_{nameof(RealEstate)}_{nameof(RealEstate.InitialLandPricePerSqFt)}"]),
					CurrentLandPricePerSqFt = double.Parse(form[$"{nameof(Collateral)}_{nameof(RealEstate)}_{nameof(RealEstate.CurrentLandPricePerSqFt)}"]),
					YearBuilt = int.Parse(form[$"{nameof(Collateral)}_{nameof(RealEstate)}_{nameof(RealEstate.YearBuilt)}"]),
					InitialStructurePrice = double.Parse(form[$"{nameof(Collateral)}_{nameof(RealEstate)}_{nameof(RealEstate.InitialStructurePrice)}"]),
					CurrentStructurePrice = double.Parse(form[$"{nameof(Collateral)}_{nameof(RealEstate)}_{nameof(RealEstate.CurrentStructurePrice)}"])
				};

			}
			else
				throw new NotImplementedException();
		}

		public static string GetLoanJson(IFormCollection form)
		{
			return JsonSerializer.Serialize(GetLoan(form));
		}

		public static string GetCollateralJson(IFormCollection form)
		{
			return JsonSerializer.Serialize(GetCollateral(form));
		}
	}
}
