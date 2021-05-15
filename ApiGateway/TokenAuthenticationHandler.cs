using ApiGateway.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApiGateway
{
	public class TokenAuthenticationHandlerOptions : AuthenticationSchemeOptions
	{
		public string Authority { get; set; }
	}

	public class TokenAuthenticationHandler : AuthenticationHandler<TokenAuthenticationHandlerOptions>
	{
		public const string TokenAuthenticationScheme = "Basic";

		private IHttpClientFactory _httpClientFactory;

		public TokenAuthenticationHandler(IHttpClientFactory httpClientFactory, IOptionsMonitor<TokenAuthenticationHandlerOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
		{
			_httpClientFactory = httpClientFactory;
		}

		protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
		{
			if (!Request.Headers.ContainsKey("Authorization")) 
				return AuthenticateResult.Fail("No Authorization Header");

			string authorizationHeader = Request.Headers["Authorization"];
			if (string.IsNullOrEmpty(authorizationHeader) ||
				!authorizationHeader.StartsWith("Bearer") ||
				string.IsNullOrEmpty(authorizationHeader.Substring("Bearer".Length).Trim()))
				return AuthenticateResult.Fail("Invalid Authorization Header");

			if (!Request.Headers.ContainsKey("UserId"))
				return AuthenticateResult.Fail("No User Id");

			string userId = Request.Headers["UserId"];
			string accessToken = authorizationHeader.Substring("Bearer".Length).Trim();
			string refreshToken = Request.Headers.ContainsKey("RefreshToken") ? Request.Headers["RefreshToken"] : "";

			TokenValidationRequestBody validationRequestBody = new TokenValidationRequestBody()
			{
				UserId = userId,
				AccessToken = accessToken,
				RefreshIfExpired = !string.IsNullOrEmpty(refreshToken),
				RefreshToken = refreshToken
			};

			Logger.LogInformation(JsonSerializer.Serialize(validationRequestBody));//TODO: remove log

			using (HttpClient client = _httpClientFactory.CreateClient())
			{
				HttpRequestMessage validationRequest = new HttpRequestMessage()
				{
					Method = HttpMethod.Post,
					RequestUri = new Uri(Options.Authority),
					Content = new StringContent(JsonSerializer.Serialize(validationRequestBody), Encoding.UTF8, "application/json")
				};

				HttpResponseMessage validationResponse;
				try { validationResponse = await client.SendAsync(validationRequest); }
				catch (HttpRequestException) { return AuthenticateResult.Fail("Unable to connect with AuthorizationApi"); }

				if (validationResponse.StatusCode == HttpStatusCode.BadRequest)
					return AuthenticateResult.Fail("BadRequest to AuthorizationApi");
				if (validationResponse.StatusCode == HttpStatusCode.InternalServerError) 
					return AuthenticateResult.Fail("Something went wrong in AuthorizationApi");

				TokenValidationResponseBody validationResponseBody = JsonSerializer.Deserialize<TokenValidationResponseBody>(await validationResponse.Content.ReadAsStringAsync(), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

				Logger.LogInformation(JsonSerializer.Serialize(validationResponseBody));//TODO: remove log

				if (!validationResponseBody.IsValid)
					return AuthenticateResult.Fail("Invalid Access Token");

				if (validationResponseBody.IsRefreshed)
					Request.Headers["Authorization"] = "Bearer " + validationResponseBody.NewAccessToken;

				ClaimsPrincipal principal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, userId) }, TokenAuthenticationScheme));
				AuthenticationTicket ticket = new AuthenticationTicket(principal, TokenAuthenticationScheme);

				return AuthenticateResult.Success(ticket);
			}
		}
	}
}
