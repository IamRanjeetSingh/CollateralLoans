using RiskAssessmentApi.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RiskAssessmentApi.Services
{
	/// <summary>
	/// Concrete implementation of <see cref="IRiskAssessment"/>
	/// </summary>
	public class RiskAssessment : IRiskAssessment
	{
		/// <summary>
		/// Communicate with the LoanManagementApi using this service
		/// </summary>
		private ILoanManagement _loanManagement;

		/// <summary>
		/// Communicate with the CollateralManagementApi using this service
		/// </summary>
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
			if (loan == null)
				return null;

			List<Collateral> collaterals = await collateralsTask;
			if (collaterals.Count == 0)
				return null;

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
				LoanValue = totalCollateralValue,//TODO: change the loan value, use right formula to calculate the final loan amount
				TotalCollateralValue = totalCollateralValue,
				LastAssessDate = lastAssessDate
			};
		}
	}
}
