using CollateralManagementApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
		/// <exception cref="ArgumentNullException"><see cref="CollateralDb"/> parameter is null</exception>
		List<Collateral> GetAll(Page page, Filter filter, CollateralDb db);

		Collateral GetById(CollateralDb db, int id);

		int Save(Collateral collateral, CollateralDb db);
	}
}
