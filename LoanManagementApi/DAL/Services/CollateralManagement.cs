using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LoanManagementApi.DAL.Services
{
	public class CollateralManagement : ICollateralManagement
	{
		private IHttpClientFactory _httpClientFactory;
		private readonly string _collateralBaseUrl;
		private ILogger<CollateralManagement> _logger;

		public CollateralManagement(IHttpClientFactory httpClientFactory, string collateralBaseUrl, ILogger<CollateralManagement> logger)
		{
			_httpClientFactory = httpClientFactory;
			_collateralBaseUrl = collateralBaseUrl;
			_logger = logger;
		}

		public async Task<HttpResponseMessage> Save(JsonElement collaterals)
		{
			using (HttpClient client = _httpClientFactory.CreateClient())
			{
				HttpRequestMessage request = new HttpRequestMessage()
				{
					Method = HttpMethod.Post,
					RequestUri = new Uri(_collateralBaseUrl + "/api/collateral/collection"),
					Content = new StringContent(collaterals.GetRawText(), Encoding.UTF8, "application/json")
				};

				return await client.SendAsync(request);
			}
		}
	}
}
