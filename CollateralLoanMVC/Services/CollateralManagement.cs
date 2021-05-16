using CollateralLoanMVC.Exceptions;
using CollateralLoanMVC.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace CollateralLoanMVC.Services
{
	public class CollateralManagement : ICollateralManagement
	{
		private IHttpClientFactory _clientFactory;
		private string _collateralApiBaseUrl;
		private ILogger<CollateralManagement> _logger;

		public CollateralManagement(IHttpClientFactory clientFactory, string collateralApiBaseUrl, ILogger<CollateralManagement> logger)
		{
			_clientFactory = clientFactory;
			_collateralApiBaseUrl = collateralApiBaseUrl;
			_logger = logger;
		}

		public async Task<List<Collateral>> GetByLoanId(int loanId)
		{
			if (loanId <= 0)
				throw new ArgumentException("loan id less than or equal to zero");

			using(HttpClient client = _clientFactory.CreateClient())
			{
				HttpRequestMessage request = new HttpRequestMessage()
				{
					Method = HttpMethod.Get,
					RequestUri = new Uri($"{_collateralApiBaseUrl}/api/collateral?pageno=1&pagesize=100&loanid={loanId}"),
				};

				HttpResponseMessage response;
				response = await client.SendAsync(request);

				if (!response.IsSuccessStatusCode)
				{
					_logger.LogInformation("something went wrong in CollateralManagementApi. StatusCode: "+response.StatusCode);
					throw new UnexpectedResponseException("something went wrong in CollateralManagementApi");
				}

				JsonElement responseBodyJson = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;
				if (responseBodyJson.ValueKind != JsonValueKind.Array)
				{
					_logger.LogInformation("response body was not an json array");
					throw new UnexpectedResponseException("response body was not an json array");
				}

				List<Collateral> collaterals = new List<Collateral>();
				for (int index = 0; index < responseBodyJson.GetArrayLength(); index++)
				{
					JsonElement collateralJson = responseBodyJson[index];
					if (collateralJson.ValueKind != JsonValueKind.Object)
					{
						_logger.LogInformation($"ignoring collateral at index {index}, value kind is not json object");
						continue;
					}

					JsonElement type;
					if (!collateralJson.TryGetProperty(nameof(Collateral.Type).Trim(), out type) && !collateralJson.TryGetProperty(nameof(Collateral.Type).ToLower().Trim(), out type))
					{
						_logger.LogInformation($"ignoring collateral at index {index}, no type property found");
						continue;
					}
					if(type.ValueKind != JsonValueKind.String)
					{
						_logger.LogInformation($"ignoring collateral at index {index}, type property is not a json string");
						continue;
					}

					JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
					if (type.GetString().ToLower() == nameof(Land).ToLower())
						collaterals.Add(JsonSerializer.Deserialize<Land>(collateralJson.GetRawText(), jsonSerializerOptions));
					else if (type.GetString().ToLower() == nameof(RealEstate).ToLower())
						collaterals.Add(JsonSerializer.Deserialize<RealEstate>(collateralJson.GetRawText(), jsonSerializerOptions));
					else
					{
						_logger.LogInformation($"ignoring collateral at index {index}, unknown type: {type}");
						continue;
					}
				}

				return collaterals;
			}
		}

		public async Task<JsonElement> GetCollateral(int collateralId)
		{
			if (collateralId <= 0)
				throw new ArgumentException("collateralId <= 0");

			using(HttpClient client = _clientFactory.CreateClient())
			{
				HttpRequestMessage request = new HttpRequestMessage()
				{
					Method = HttpMethod.Get,
					RequestUri = new Uri($"{_collateralApiBaseUrl}/api/collateral/{collateralId}")
				};

				HttpResponseMessage response = await client.SendAsync(request);

				if (response.StatusCode == HttpStatusCode.NotFound)
					throw new NullReferenceException($"no collateral found by id {collateralId}");
				if (response.StatusCode != HttpStatusCode.OK)
					throw new UnexpectedResponseException($"unexpected response from CollateralManagementApi. Status Code: {response.StatusCode}");

				JsonElement collateralJson = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;

				if (collateralJson.ValueKind != JsonValueKind.Object)
					throw new UnexpectedResponseException("collateral json value kind is not json object");
				
				JsonElement type;
				if (!collateralJson.TryGetProperty(nameof(Collateral.Type).Trim(), out type) && !collateralJson.TryGetProperty(nameof(Collateral.Type).ToLower().Trim(), out type))
					throw new UnexpectedResponseException("no type property found");
				if (type.ValueKind != JsonValueKind.String)
					throw new UnexpectedResponseException("type property is not of type json string");

				_logger.LogInformation(collateralJson.GetRawText());

				return collateralJson;

				//JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
				//if (type.GetString().ToLower() == nameof(Land).ToLower())
				//	return JsonSerializer.Deserialize<Land>(collateralJson.GetRawText(), jsonSerializerOptions);
				//else if (type.GetString().ToLower() == nameof(RealEstate).ToLower())
				//	return JsonSerializer.Deserialize<RealEstate>(collateralJson.GetRawText(), jsonSerializerOptions);
				//else
				//	throw new UnexpectedResponseException($"type: {type.GetString().ToLower()} is unknown");
			}
		}
	}
}
