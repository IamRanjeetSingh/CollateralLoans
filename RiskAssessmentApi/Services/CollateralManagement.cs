using RiskAssessmentApi.Exceptions;
using RiskAssessmentApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace RiskAssessmentApi.Services
{
	public class CollateralManagement : ICollateralManagement
	{
		private IHttpClientFactory _httpClientFactory;
		private string _collateralApiBaseUrl;

		public CollateralManagement(IHttpClientFactory httpClientFactory, string collateralApiBaseUrl)
		{
			_httpClientFactory = httpClientFactory;
			_collateralApiBaseUrl = collateralApiBaseUrl;
		}

		public async Task<List<Collateral>> GetByLoanIdAsync(int loanId)
		{
			using(HttpClient client = _httpClientFactory.CreateClient())
			{
				HttpRequestMessage request = new HttpRequestMessage()
				{
					Method = HttpMethod.Get,
					RequestUri = new Uri($"{_collateralApiBaseUrl}/api/collateral?pageNo=1&pageSize={100}&loanId={loanId}")
				};
				HttpResponseMessage response = await client.SendAsync(request);
				if (response.StatusCode != HttpStatusCode.OK) throw new UnexpectedResponseException($"CollateralManagementApi response: {response.StatusCode}");

				JsonElement collateralsJson = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;
				if (collateralsJson.ValueKind != JsonValueKind.Array) throw new UnexpectedResponseException($"CollateralManagementApi response is not an array");

				List<Collateral> collaterals = new List<Collateral>(collateralsJson.GetArrayLength());
				for (int index = 0, length = collateralsJson.GetArrayLength(); index < length; index++)
				{
					JsonElement collateralJson = collateralsJson[index];
					if (collateralJson.ValueKind != JsonValueKind.Object)
						continue;

					collaterals.Add(JsonSerializer.Deserialize<Collateral>(collateralJson.GetRawText(), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }));
				}

				return collaterals;
			}
		}
	}
}
