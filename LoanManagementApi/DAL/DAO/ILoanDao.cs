using LoanManagementApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoanManagementApi.DAL.DAO
{
	public interface ILoanDao
	{
		/// <summary>
		/// Get a list of <see cref="Loan"/>, paginated and filtered.
		/// </summary>
		/// <param name="page">page details</param>
		/// <param name="filter">filters to be applied</param>
		/// <param name="db">data source to be searched</param>
		/// <returns>list of <see cref="Loan"/></returns>
		/// <exception cref="ArgumentNullException"><see cref="LoanDb"/> parameter is null</exception>
		List<Loan> GetAll(Page page, Filter filter, LoanDb db);

		/// <summary>
		/// Get a <see cref="Loan"/> instance associated with the specified id.
		/// </summary>
		/// <param name="id">id of the loan to fetched</param>
		/// <param name="db">data source to be searched</param>
		/// <returns><see cref="Loan"/> instance associated with the given id or null if no match found</returns>
		/// <exception cref="ArgumentNullException"><see cref="LoanDb"/> parameter is null</exception>
		/// <exception cref="InvalidOperationException">more than one insntances found for given id</exception>
		Loan GetById(int id, LoanDb db);

		int Save(Loan loan, LoanDb db);
		int UpdateFull(int id, Loan loan, LoanDb db);
		int UpdatePartial(int id, dynamic loan, LoanDb db);
		int Delete(int id, LoanDb db);
	}
}
