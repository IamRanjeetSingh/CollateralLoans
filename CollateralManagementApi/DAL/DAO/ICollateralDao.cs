using CollateralManagementApi.Models;
using System;
using System.Collections.Generic;

namespace CollateralManagementApi.DAL.DAO
{
	public interface ICollateralDao
	{
		/// <summary>
		/// Get a list of <see cref="Collateral"/>, paginated and filtered.
		/// </summary>
		/// <param name="page">page details</param>
		/// <param name="filter">value to filter the list upon</param>
		/// <param name="db">data source to be searched</param>
		/// <returns>list of <see cref="Collateral"/>. This list can be empty</returns>
		/// <exception cref="ArgumentNullException">page, filter or db is null</exception>
		/// <exception cref="ArgumentException">page no is less than equal to zero or page size is less than or equal to zero or sort property is unknown</exception>
		List<Collateral> GetAll(Page page, Filter filter, CollateralDb db);

		/// <summary>
		/// Get <see cref="Collateral"/> associated with the given id.
		/// </summary>
		/// <param name="id">id for which the collateral will be searched</param>
		/// <param name="db">data source to be searched</param>
		/// <returns><see cref="Collateral"/> associated with the given id or null if no collateral found for the given id</returns>
		/// <exception cref="ArgumentNullException">db is null</exception>
		/// <exception cref="ArgumentException">id is less than or equal to zero</exception>
		Collateral GetById(int id, CollateralDb db);

		/// <summary>
		/// Save <see cref="Collateral"/> in the given data source.
		/// </summary>
		/// <param name="collateral">collateral to be saved</param>
		/// <param name="db">data source where the collateral will be saved</param>
		/// <returns>id of the newly saved collateral.</returns>
		/// <exception cref="ArgumentNullException">collateral or db is null</exception>
		/// <exception cref="ArgumentException">loanId or customerId for the collateral are less than or equal to zero</exception>
		int Save(Collateral collateral, CollateralDb db);
	}
}
