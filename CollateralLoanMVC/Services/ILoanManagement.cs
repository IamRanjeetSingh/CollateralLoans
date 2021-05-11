using CollateralLoanMVC.Util;
using CollateralLoanMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollateralLoanMVC.Services
{
	/// <summary>
	/// This service is responsible for communicating with the Loan Management Api.
	/// </summary>
	public interface ILoanManagement
	{
		/// <summary>
		/// Gets a list of <see cref="Loan"/> filtered and paginated.
		/// </summary>
		/// <param name="page">details about the page</param>
		/// <param name="filter">values to filter the loans upon</param>
		/// <returns>list of loans</returns>
		Task<List<Loan>> GetAll(Page page, LoanFilter filter);

		/// <summary>
		/// Gets an individual <see cref="Loan"/> instance.
		/// </summary>
		/// <param name="loanId">id of the loan to be fetch</param>
		/// <returns>loan associated with the given id or null if an error occurs or no loan found for the given id</returns>
		Task<Loan> Get(int loanId);

		/// <summary>
		/// Saves the given <see cref="Loan"/> instance.
		/// </summary>
		/// <param name="loan">loan instance to be saved</param>
		/// <returns>true if the loan was saved successfully, false otherwise</returns>
		Task<bool> Save(Loan loan);

		/// <summary>
		/// Deletes the loan associated with the given id.
		/// </summary>
		/// <param name="loanId">id of the loan to be deleted</param>
		/// <returns>true if the loan was deleted successfully, false otherwise</returns>
		Task<bool> Delete(int loanId);
	}
}
