using RiskAssessmentApi.Models;
using System.Threading.Tasks;

namespace RiskAssessmentApi.Services
{
	public interface IRiskAssessment
	{
		Task<Risk> EvaluateAsync(int loanId);
	}
}
