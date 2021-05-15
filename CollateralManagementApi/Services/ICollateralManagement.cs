using CollateralManagementApi.DAL;
using CollateralManagementApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace CollateralManagementApi.Services
{
	public interface ICollateralManagement
	{
		List<Collateral> GetAll(Page page, Filter filter);

		Collateral GetById(int id);

		int Save(JsonElement collateralJson);

		List<int> SaveMultiple(JsonElement collateralArrayJson);
	}
}
