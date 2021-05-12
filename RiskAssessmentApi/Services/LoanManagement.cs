using RiskAssessmentApi.Exceptions;
using RiskAssessmentApi.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace RiskAssessmentApi.Services
{
	/// <summary>
	/// Concrete implementation of <see cref="ILoanManagement"/>
	/// </summary>
	public class LoanManagement : ILoanManagement
	{
		/// <summary>
		/// Base url for LoanManagementApi
		/// </summary>
		private string _loanApiBaseUrl;

		/// <summary>
		/// Use this factory class to instantiate <see cref="HttpClient"/> instances rather than creating them explicitly. 
		/// It helps in avoiding resource management related issues.
		/// </summary>
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

				if (response.StatusCode == HttpStatusCode.NotFound) return null;
				if (response.StatusCode != HttpStatusCode.OK) throw new UnexpectedResponseException("something went wrong in LoanManagementApi");

				return JsonSerializer.Deserialize<Loan>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
			}
		}
	}
}
