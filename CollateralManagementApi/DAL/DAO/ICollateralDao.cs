using CollateralManagementApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollateralManagementApi.DAL.DAO
{
	public interface ICollateralDao
	{
		List<Collateral> GetAll(CollateralDb db, Page page, Filter filter);

		Collateral GetById(CollateralDb db, int id);

		int Save(Collateral collateral, CollateralDb db);
	}
}
