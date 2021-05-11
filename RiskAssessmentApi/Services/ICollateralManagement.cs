using RiskAssessmentApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RiskAssessmentApi.Services
{
	public interface ICollateralManagement
	{
		Task<List<Collateral>> GetByLoanIdAsync(int loanId);
	}
}
