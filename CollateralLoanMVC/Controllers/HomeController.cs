using CollateralLoanMVC.Views.Home;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CollateralLoanMVC.Controllers
{
	[Route("[controller]")]
	public class HomeController : Controller
	{
		/// <summary>
		/// Get the template view for index page. This page does not provide the list of loans.
		/// </summary>
		/// <returns>template view for index page</returns>
		[HttpGet("[action]")]
		public ActionResult Index()
		{
			return View();
		}

		//TODO: Remove debug action
		[HttpPost("[action]")]
		public IActionResult Test(List<TestUser> testUsers)
		{
			return Ok(new { result = testUsers });
		}

		//TODO: Remove debug action
		[HttpGet("[action]")]
		public IActionResult Test()
		{
			return View();
		}
	}
}
