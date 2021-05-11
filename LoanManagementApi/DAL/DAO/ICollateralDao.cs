using LoanManagementApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace LoanManagementApi.DAL.DAO
{
	public interface ICollateralDao
	{
		Task<HttpResponseMessage> Save(JsonElement collaterals);
	}
}
