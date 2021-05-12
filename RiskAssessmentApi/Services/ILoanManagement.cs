using RiskAssessmentApi.Exceptions;
using RiskAssessmentApi.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace RiskAssessmentApi.Services
{
	public interface ILoanManagement
	{
		/// <summary>
		/// Get a <see cref="Loan"/> instance associated with the specified id.
		/// </summary>
		/// <param name="id">id of the loan to fetched</param>
		/// <returns><see cref="Loan"/> instance associated with the given id or null if no match found</returns>
		/// <exception cref="HttpRequestException">cannot communicate with LoanManagementApi</exception>
		/// <exception cref="UnexpectedResponseException">something went wrong in LoanManagementApi</exception>
		Task<Loan> GetAsync(int id);
	}
}
