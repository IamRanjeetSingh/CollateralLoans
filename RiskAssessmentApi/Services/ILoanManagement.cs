using RiskAssessmentApi.Models;
using System.Threading.Tasks;

namespace RiskAssessmentApi.Services
{
	public interface ILoanManagement
	{
		Task<Loan> GetAsync(int id);
	}
}
