using CollateralManagementApi.Models;
using CollateralManagementApi.Util;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CollateralManagementApiTests
{
	[TestFixture]
	public class CollateralDeserializerTests
	{
		private CollateralDeserializer collateralDeserializer;

		[OneTimeSetUp]
		public void SetupTests()
		{
			collateralDeserializer = new CollateralDeserializer();
		}

		[Test]
		public void DeserializeByType_InvalidCollateralJson_ShouldThrowArgumentException()
		{
			Assert.Throws<ArgumentException>(() => collateralDeserializer.DeserializeByType(new JsonElement(), nameof(Collateral.Type)));
		}

		[Test]
		public void DeserializeByType_ValidLandCollateralJson_ShouldReturnCollateral()
		{
			Land validLandCollateral = new Land()
			{
				LoanId = 1001,
				CustomerId = 2001,
				InitialAssesDate = DateTime.Now.AddDays(-5),
				LastAssessDate = DateTime.Now.AddDays(5),
				AreaInSqFt = 1000,
				InitialPricePerSqFt = 1000,
				CurrentPricePerSqFt = 2000
			};
			JsonElement validLandCollateralJson = JsonDocument.Parse(JsonSerializer.Serialize(validLandCollateral)).RootElement;
			Land c = (Land)collateralDeserializer.DeserializeByType(validLandCollateralJson, nameof(Collateral.Type));

			Collateral collateral1 = new Land()
			{
				LoanId = 1001,
				CustomerId = 2001,
				InitialAssesDate = DateTime.Now.AddDays(-5),
				LastAssessDate = DateTime.Now.AddDays(5),
				AreaInSqFt = 1000,
				InitialPricePerSqFt = 1000,
				CurrentPricePerSqFt = 2000
			};
			string collateralString1 = JsonSerializer.Serialize(validLandCollateral);
			JsonElement collateralJsonElement = JsonDocument.Parse(collateralString1).RootElement;
			string collateralString2 = collateralJsonElement.GetRawText();
			Collateral collateral2 = JsonSerializer.Deserialize<Land>(collateralString2, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
			Collateral collateral3 = Newtonsoft.Json.JsonConvert.DeserializeObject<Land>(collateralString2);


			throw new Exception(JsonSerializer.Serialize(collateral4));
		}
	}
}
