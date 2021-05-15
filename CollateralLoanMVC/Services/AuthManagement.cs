using CollateralLoanMVC.DTO;
using CollateralLoanMVC.Exceptions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CollateralLoanMVC.Services
{
	public class AuthManagement : IAuthManagement
	{
		private IHttpClientFactory _httpClientFactory;
		private string _authApiBaseUrl;
		private ILogger<AuthManagement> _logger;

		public AuthManagement(IHttpClientFactory httpClientFactory, string authEndpoint, ILogger<AuthManagement> logger)
		{
			_httpClientFactory = httpClientFactory;
			_authApiBaseUrl = authEndpoint;
			_logger = logger;
		}

		public async Task<AuthenticationResponse> Login(string userId, string password)
		{
			if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(password))
				throw new ArgumentException("userid or password is null or empty");

			using(HttpClient client = _httpClientFactory.CreateClient())
			{
				AuthenticationRequest requestBody = new AuthenticationRequest()
				{
					UserId = userId,
					Password = password
				};
				HttpRequestMessage request = new HttpRequestMessage()
				{
					Method = HttpMethod.Post,
					RequestUri = new Uri($"{_authApiBaseUrl}/api/auth"),
					Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json")
				};

				HttpResponseMessage response = await client.SendAsync(request);

				if (!response.IsSuccessStatusCode)
					throw new UnexpectedResponseException("something went wrong in AuthorizationApi. StatusCode: "+response.StatusCode);

				AuthenticationResponse responseBody = JsonSerializer.Deserialize<AuthenticationResponse>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
				
				return responseBody;
			}
		}
	}
}

//return Task.FromResult(new AuthenticationResponse()
//{
//	IsSuccessful = true,
//	AccessToken = "AccessToken_" + DateTime.UtcNow,
//	AccessTokenExpiresIn = DateTime.UtcNow.AddMinutes(15),
//	RefreshToken = "RefreshToken_" + DateTime.UtcNow,
//	RefreshTokenExpiresIn = DateTime.UtcNow.AddMinutes(100)
//});