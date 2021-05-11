﻿using CollateralLoanMVC.Exceptions;
using CollateralLoanMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace CollateralLoanMVC.Services
{
	/// <summary>
	/// Concrete implementation of <see cref="IRiskAssessment"/>.
	/// </summary>
	public class RiskAssessment : IRiskAssessment
	{

		/// <summary>
		/// Base url for Risk Assessment Api, eg. https://localhost:5001. This url is specified in appsettings.json
		/// </summary>
		private readonly string _riskApiBaseUrl;

		/// <summary>
		/// Used to instantiate a new <see cref="HttpClient"/> instance rather than creating it directly. It helps in avoiding resource management 
		/// socket exhaustion.
		/// </summary>
		private IHttpClientFactory _httpClientFactory;

		public RiskAssessment(string riskApiBaseUrl, IHttpClientFactory httpClientFactory)
		{
			_riskApiBaseUrl = riskApiBaseUrl;
			_httpClientFactory = httpClientFactory;
		}

		public async Task<Risk> Get(int loanId)
		{
			using (HttpClient client = _httpClientFactory.CreateClient())
			{
				HttpRequestMessage request = new HttpRequestMessage()
				{
					Method = HttpMethod.Get,
					RequestUri = new Uri($"{_riskApiBaseUrl}/api/risk/{loanId}")
				};
				HttpResponseMessage response = await client.SendAsync(request);
				if (response.StatusCode != HttpStatusCode.OK) throw new UnexpectedResponseException($"RiskManagementApi response: {response.StatusCode}");

				JsonElement riskJson = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;
				if (riskJson.ValueKind != JsonValueKind.Object) throw new UnexpectedResponseException($"RiskManagementApi response is not an Object");

				return JsonSerializer.Deserialize<Risk>(riskJson.GetRawText(), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
			}
		}
	}
}
