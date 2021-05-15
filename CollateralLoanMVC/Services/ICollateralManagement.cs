using CollateralLoanMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollateralLoanMVC.Services
{
	public interface ICollateralManagement
	{
		Task<List<Collateral>> GetByLoanId(int loanId);

		Task<Collateral> GetCollateral(int collateralId);
	}
}
