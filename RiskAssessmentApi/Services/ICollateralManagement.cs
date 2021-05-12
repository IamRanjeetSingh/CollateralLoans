using RiskAssessmentApi.Exceptions;
using RiskAssessmentApi.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace RiskAssessmentApi.Services
{
	public interface ICollateralManagement
	{
		/// <summary>
		/// Get a list of <see cref="Collateral"/> for the given loan id.
		/// </summary>
		/// <param name="loanId">loan id for against which the collaterals will be searched</param>
		/// <returns>list of <see cref="Collateral"/>. This list can be empty</returns>
		/// <exception cref="HttpRequestException">cannot communicate with CollateralManagementApi</exception>
		/// <exception cref="UnexpectedResponseException">unexpected response from CollateralManagementApi</exception>
		Task<List<Collateral>> GetByLoanIdAsync(int loanId);
	}
}
