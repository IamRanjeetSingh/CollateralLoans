using CollateralLoanMVC.DTO;
using CollateralLoanMVC.Exceptions;
using CollateralLoanMVC.Extentions;
using CollateralLoanMVC.Services;
using CollateralLoanMVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CollateralLoanMVC.Controllers
{
	[Route("[controller]")]
	public class AccountController : Controller
	{
		private IAuthManagement _authManagement;
		private string _accessTokenCookieKey = "accesstoken";
		private string _userIdSessionKey = "userid";
		private string _refreshTokenSessionKey = "refreshtoken";

		public AccountController(IAuthManagement authManagement)
		{
			_authManagement = authManagement;
		}

		[HttpGet("[action]")]
		public IActionResult Login()
		{
			if (Request.Cookies[_accessTokenCookieKey] == null)
				return View();
			else
				return Content("already logged in");
		}

		[HttpPost("[action]")]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			if (!ModelState.IsValid)
				return View();

			AuthenticationResponse authResponse;
			try { authResponse = await _authManagement.Login(model.UserId, model.Password); }
			catch (HttpRequestException) { return Ok(new { error = "unable to connect with AuthorizationApi" }); }
			catch(UnexpectedResponseException) { return Ok(new { error = "something went wrong in AuthorizationApi" }); }

			if(authResponse.IsSuccessful)
			{
				ModelState.Clear();

				Response.Cookies.Append(_accessTokenCookieKey, authResponse.AccessToken);
				HttpContext.Session.SetString(_userIdSessionKey, model.UserId);
				HttpContext.Session.SetString(_refreshTokenSessionKey, authResponse.RefreshToken);

				return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).RemoveSuffix("Controller"));
			}
			else
			{
				ModelState.AddModelError("", "Login failed");
				return View();
			}
		}

		[HttpPost("[action]")]
		public IActionResult Logout()
		{
			Response.Cookies.Delete(_accessTokenCookieKey);
			HttpContext.Session.Clear();

			return RedirectToAction(nameof(Login));
		}

		[HttpGet("[action]")]
		[Authorize]
		public IActionResult Test()
		{
			return Content("secret data");
		}
	}
}
