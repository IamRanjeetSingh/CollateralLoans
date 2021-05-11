using CollateralLoanMVC.Exceptions;
using CollateralLoanMVC.Models;
using CollateralLoanMVC.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CollateralLoanMVC.Services
{
	/// <summary>
	/// Concrete implementation of <see cref="ILoanManagement"/>.
	/// </summary>
	public class LoanManagement : ILoanManagement
	{
		/// <summary>
		/// Base url for Loan Management Api, eg. https://localhost:5001. This url is specified in appsettings.json
		/// </summary>
		private readonly string _loanApiBaseUrl;

		/// <summary>
		/// Used to instantiate a new <see cref="HttpClient"/> instance rather than creating it directly. It helps in avoiding resource management 
		/// socket exhaustion.
		/// </summary>
		private readonly IHttpClientFactory _httpClientFactory;

		public LoanManagement(string loanApiBaseUrl, IHttpClientFactory httpClientFactory)
		{
			_loanApiBaseUrl = loanApiBaseUrl;
			_httpClientFactory = httpClientFactory;
		}

		public async Task<bool> Delete(int loanId)
		{
			using(HttpClient client = _httpClientFactory.CreateClient())
			{
				HttpRequestMessage request = new HttpRequestMessage()
				{
					Method = HttpMethod.Delete,
					RequestUri = new Uri($"{_loanApiBaseUrl}/api/loan/{loanId}")
				};
				HttpResponseMessage response = await client.SendAsync(request);
				return response.StatusCode == HttpStatusCode.OK;
			}
		}

		public async Task<Loan> Get(int loanId)
		{
			using (HttpClient client = _httpClientFactory.CreateClient())
			{
				HttpRequestMessage request = new HttpRequestMessage()
				{
					Method = HttpMethod.Get,
					RequestUri = new Uri($"{_loanApiBaseUrl}/api/loan/{loanId}")
				};
				HttpResponseMessage response = await client.SendAsync(request);
				if (response.StatusCode != HttpStatusCode.OK) return null;

				return JsonSerializer.Deserialize<Loan>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
			}
		}

		public async Task<List<Loan>> GetAll(Page page, LoanFilter filter)
		{
			using (HttpClient client = _httpClientFactory.CreateClient())
			{
				HttpRequestMessage request = new HttpRequestMessage()
				{
					Method = HttpMethod.Get,
					RequestUri = new Uri($"{_loanApiBaseUrl}/api/loan?pageNo={page.PageNo}&pageSize={page.PageSize}")//TODO: Add filters to url
				};
				HttpResponseMessage response = await client.SendAsync(request);
				if (response.StatusCode != HttpStatusCode.OK) throw new UnexpectedResponseException($"LoanManagementApi response{response.StatusCode}");

				JsonElement loansJson = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;
				if (loansJson.ValueKind != JsonValueKind.Array) throw new UnexpectedResponseException($"LoanManagementApi response is not an array");

				List<Loan> loans = new List<Loan>();
				for(int index = 0, length = loansJson.GetArrayLength(); index < length; index++)
				{
					JsonElement loanJson = loansJson[index];
					if (loanJson.ValueKind != JsonValueKind.Object)
						continue;
					loans.Add(JsonSerializer.Deserialize<Loan>(loanJson.GetRawText(), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }));
				}
				return loans;
			}
		}

		public async Task<bool> Save(Loan loan)
		{
			using (HttpClient client = _httpClientFactory.CreateClient())
			{
				HttpRequestMessage request = new HttpRequestMessage()
				{
					Method = HttpMethod.Post,
					RequestUri = new Uri($"{_loanApiBaseUrl}/api/loan"),
					Content = new StringContent(JsonSerializer.Serialize(loan), Encoding.UTF8, "application/json")
				};
				HttpResponseMessage response = await client.SendAsync(request);
				return response.StatusCode == HttpStatusCode.Created;
			}
		}
	}
}
