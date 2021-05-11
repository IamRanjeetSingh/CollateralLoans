using LoanManagementApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace LoanManagementApi.DAL.DAO
{
	public class CollateralDao : ICollateralDao
	{
		private IHttpClientFactory _httpClientFactory;
		private readonly Uri _collateralEndpoint;

		public CollateralDao(IHttpClientFactory httpClientFactory, Uri collateralEndpoint)
		{
			_httpClientFactory = httpClientFactory;
			_collateralEndpoint = collateralEndpoint;
		}

		public Task<HttpResponseMessage> Save(JsonElement collaterals)
		{
			using (HttpClient client = _httpClientFactory.CreateClient())
			{
				return Task.FromResult(new HttpResponseMessage() { StatusCode = HttpStatusCode.OK });//Dummy code

			}
		}
	}
}
