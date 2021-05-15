using CollateralManagementApi.Models;
using System;
using System.Text.Json;

namespace CollateralManagementApi.Util
{
	public class CollateralDeserializer : ICollateralDeserializer
	{
		public Collateral DeserializeByType(JsonElement collateralJson, string typePropertyName)
		{
			if (collateralJson.ValueKind != JsonValueKind.Object) 
				throw new ArgumentException($"Provided json is not a {JsonValueKind.Object.GetType().FullName}");
			if (!collateralJson.TryGetProperty(typePropertyName, out JsonElement typeJson) && !collateralJson.TryGetProperty(typePropertyName.ToLower(), out typeJson)) 
				throw new ArgumentException($"No property found by {typePropertyName} name");
			if (typeJson.ValueKind != JsonValueKind.String) 
				throw new ArgumentException($"{typePropertyName} is not of type {JsonValueKind.String.GetType().FullName}");

			return InitializeCollateral(typeJson.GetString(), collateralJson);
		}

		private Collateral InitializeCollateral(string type, JsonElement json)
		{
			if (json.ValueKind != JsonValueKind.Object) throw new ArgumentException($"Provided json is not a {JsonValueKind.Object.GetType().FullName}");
			JsonSerializerOptions options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
			return type switch
			{
				CollateralType.Land => JsonSerializer.Deserialize<Land>(json.GetRawText(), options),
				CollateralType.RealEstate => JsonSerializer.Deserialize<RealEstate>(json.GetRawText(), options),
				_ => throw new ArgumentException($"Unsupported Collateral type {type}")
			};
		}
	}
}
