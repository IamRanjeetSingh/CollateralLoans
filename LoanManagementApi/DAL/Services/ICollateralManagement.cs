using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace LoanManagementApi.DAL.Services
{
	public interface ICollateralManagement
	{
		Task<HttpResponseMessage> Save(JsonElement collaterals);
	}
}
