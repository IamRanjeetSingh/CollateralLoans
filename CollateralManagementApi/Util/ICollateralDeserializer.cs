using CollateralManagementApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace CollateralManagementApi.Util
{
	public interface ICollateralDeserializer
	{
		Collateral DeserializeByType(JsonElement collateralJson, string typePropertyName);
	}
}
