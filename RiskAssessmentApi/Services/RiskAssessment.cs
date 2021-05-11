using RiskAssessmentApi.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RiskAssessmentApi.Services
{
	public class RiskAssessment : IRiskAssessment
	{
		private ILoanManagement _loanManagement;
		private ICollateralManagement _collateralManagement;

		public RiskAssessment(ILoanManagement loanManagement, ICollateralManagement collateralManagement)
		{
			_loanManagement = loanManagement;
			_collateralManagement = collateralManagement;
		}

		public async Task<Risk> EvaluateAsync(int loanId)
		{
			Task<Loan> loanTask = _loanManagement.GetAsync(loanId);
			Task<List<Collateral>> collateralsTask = _collateralManagement.GetByLoanIdAsync(loanId);
			await Task.WhenAll(loanTask, collateralsTask);

			Loan loan = await loanTask;
			List<Collateral> collaterals = await collateralsTask;

			double totalCollateralValue = 0;
			DateTime lastAssessDate = DateTime.MaxValue;
			foreach (Collateral collateral in collaterals)
			{
				totalCollateralValue += collateral.CurrentValue;
				if (collateral.LastAssessDate < lastAssessDate)
					lastAssessDate = collateral.LastAssessDate;
			}

			return new Risk()
			{
				LoanValue = totalCollateralValue,//change this
				TotalCollateralValue = totalCollateralValue,
				LastAssessDate = lastAssessDate
			};
		}
	}
}
