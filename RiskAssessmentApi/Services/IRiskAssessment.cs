using RiskAssessmentApi.Exceptions;
using RiskAssessmentApi.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace RiskAssessmentApi.Services
{
	public interface IRiskAssessment
	{
		/// <summary>
		/// Get a <see cref="Risk"/> instance for the loan associated with the given id.
		/// </summary>
		/// <param name="loanId">id of the loan for which risk details will be returned</param>
		/// <returns><see cref="Risk"/> instance for the requested loan or null if no loan or collaterals found for the given loan id</returns>
		/// <exception cref="HttpRequestException">either LoanManagementApi or CollateralManagementApi unavailable</exception>
		/// <exception cref="UnexpectedResponseException">unexpected response from either LoanManagementApi or CollateralManagementApi</exception>
		Task<Risk> EvaluateAsync(int loanId);
	}
}
