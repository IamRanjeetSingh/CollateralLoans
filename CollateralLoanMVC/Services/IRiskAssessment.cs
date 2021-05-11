using CollateralLoanMVC.Models;
using CollateralLoanMVC.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollateralLoanMVC.Services
{
	/// <summary>
	/// This service is responsible for communicating with the Risk Assessment Api.
	/// </summary>
	public interface IRiskAssessment
	{
		/// <summary>
		/// Gets <see cref="Risk"/> details for the <see cref="Loan"/> associated with the given loan id.
		/// </summary>
		/// <param name="loanId"></param>
		/// <returns>risk details for the loan or null if an error occurs or no loan found for the specified id</returns>
		Task<Risk> Get(int loanId);
	}
}
