using CollateralLoanMVC.Util;
using CollateralLoanMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using CollateralLoanMVC.Exceptions;
using System.Text.Json;

namespace CollateralLoanMVC.Services
{
	/// <summary>
	/// This service is responsible for communicating with the Loan Management Api.
	/// </summary>
	public interface ILoanManagement
	{
		/// <summary>
		/// Get a list of <see cref="Loan"/>, filtered and paginated.
		/// </summary>
		/// <param name="page">details about the page</param>
		/// <param name="filter">values to filter the loans upon</param>
		/// <returns>list of <see cref="Loan"/></returns>
		/// <exception cref="HttpRequestException">cannot communicate with LoanManagementApi</exception>
		/// <exception cref="UnexpectedResponseException">unexpected response from LoanManagementApi</exception>
		Task<List<Loan>> GetAll(Page page, LoanFilter filter);

		/// <summary>
		/// Get an individual <see cref="Loan"/> instance.
		/// </summary>
		/// <param name="loanId">id of the loan to be fetched</param>
		/// <returns>loan associated with the given id or null if an error occurs or no loan found for the given id</returns>
		/// <exception cref="HttpRequestException">cannot communicate with LoanManagementApi</exception>
		/// <exception cref="UnexpectedResponseException">unexpected response from LoanManagementApi</exception>
		Task<Loan> Get(int loanId);

		/// <summary>
		/// Saves the given <see cref="Loan"/> instance.
		/// </summary>
		/// <param name="loan">loan instance to be saved</param>
		/// <returns>true if the loan was saved successfully, false otherwise</returns>
		Task<bool> Save(Loan loan);

		/// <summary>
		/// Pass the loan and collaterals to be saved to LoanManagementApi.
		/// </summary>
		/// <param name="loan">loan to be saved</param>
		/// <param name="collaterals">collaterals array to be saved</param>
		/// <returns>true if the whole request was successful, false otherwise</returns>
		/// <exception cref="HttpRequestException">unable to connect with LoanManagementApi</exception>
		/// <exception cref="UnexpectedResponseException">unexpected response received from LoanManagementApi</exception>
		Task<bool> SaveWithCollaterals(JsonElement loan, JsonElement collaterals);

		/// <summary>
		/// Deletes the loan associated with the given id.
		/// </summary>
		/// <param name="loanId">id of the loan to be deleted</param>
		/// <returns>true if the loan was deleted successfully, false otherwise</returns>
		Task<bool> Delete(int loanId);
	}
}
