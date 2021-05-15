using CollateralLoanMVC.DTO;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace CollateralLoanMVC.Util
{
	public class TokenAuthenticationHandlerOptions : AuthenticationSchemeOptions
	{ 
		public string Authority { get; set; }
	}

	public class TokenAuthenticationHandler : AuthenticationHandler<TokenAuthenticationHandlerOptions>
	{
		private string _accessTokenCookieKey = "accesstoken";
		private string _userIdSessionKey = "userid";
		private string _refreshTokenSessionKey = "refreshtoken";

		public const string TokenAuthenticationScheme = "TokenAuth";

		private IHttpClientFactory _httpClientFactory;

		public TokenAuthenticationHandler(IHttpClientFactory httpClientFactory, IOptionsMonitor<TokenAuthenticationHandlerOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
		{
			_httpClientFactory = httpClientFactory;
		}

		protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
		{
			string userId = Context.Session.GetString(_userIdSessionKey);
			string refreshToken = Context.Session.GetString(_refreshTokenSessionKey);
			string accessToken = Request.Cookies[_accessTokenCookieKey];

			//if user id and tokens are null, then no need to send request to authorization api
			
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
					RequestUri = new Uri(Options.Authority+"/api/token"),
					Content = new StringContent(JsonSerializer.Serialize(validationRequestBody), Encoding.UTF8, "application/json")
				};

				HttpResponseMessage validationResponse;
				try { validationResponse = await client.SendAsync(validationRequest); }
				catch (HttpRequestException) { return AuthenticateResult.Fail("Unable to connect with AuthorizationApi"); }

				if (validationResponse.StatusCode == HttpStatusCode.BadRequest)
					return AuthenticateResult.Fail("BadRequest to AuthorizationApi: "+JsonSerializer.Serialize(validationRequestBody));
				if (validationResponse.StatusCode == HttpStatusCode.InternalServerError)
					return AuthenticateResult.Fail("Something went wrong in AuthorizationApi");

				TokenValidationResponseBody validationResponseBody = JsonSerializer.Deserialize<TokenValidationResponseBody>(await validationResponse.Content.ReadAsStringAsync(), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

				Logger.LogInformation(JsonSerializer.Serialize(validationResponseBody));//TODO: remove log

				if (!validationResponseBody.IsValid)
					return AuthenticateResult.Fail("Invalid Access Token");

				if (validationResponseBody.IsRefreshed)
					Response.Cookies.Append(_accessTokenCookieKey, validationResponseBody.NewAccessToken);
					//Request.Headers["Authorization"] = "Bearer " + validationResponseBody.NewAccessToken;

				ClaimsPrincipal principal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, userId) }, TokenAuthenticationScheme));
				AuthenticationTicket ticket = new AuthenticationTicket(principal, TokenAuthenticationScheme);

				return AuthenticateResult.Success(ticket);
			}
		}
	}
}

//if (userId != null && accessToken != null && refreshToken != null)
//{
//	ClaimsPrincipal principal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, userId) }, TokenAuthenticationScheme));
//	AuthenticationTicket ticket = new AuthenticationTicket(principal, TokenAuthenticationScheme);

//	return Task.FromResult(AuthenticateResult.Success(ticket));
//}
//else
//	return Task.FromResult(AuthenticateResult.Fail("unauthorized: no access token found"));