using RiskAssessmentApi.Exceptions;
using RiskAssessmentApi.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace RiskAssessmentApi.Services
{
	public class LoanManagement : ILoanManagement
	{
		private string _loanApiBaseUrl;
		private IHttpClientFactory _httpClientFactory;

		public LoanManagement(IHttpClientFactory httpClientFactory, string loanApiBaseUrl)
		{
			_loanApiBaseUrl = loanApiBaseUrl;
			_httpClientFactory = httpClientFactory;
		}

		public async Task<Loan> GetAsync(int id)
		{
			using (HttpClient client = _httpClientFactory.CreateClient())
			{
				HttpRequestMessage request = new HttpRequestMessage()
				{
					Method = HttpMethod.Get,
					RequestUri = new Uri($"{_loanApiBaseUrl}/api/loan/{id}")
				};

				HttpResponseMessage response = await client.SendAsync(request);
				if (response.StatusCode != HttpStatusCode.OK) throw new UnexpectedResponseException($"LoanManagementApi response: {response.StatusCode}");

				return JsonSerializer.Deserialize<Loan>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
			}
		}
	}
}
