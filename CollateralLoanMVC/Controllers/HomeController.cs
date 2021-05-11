using Microsoft.AspNetCore.Mvc;
using System;

namespace CollateralLoanMVC.Controllers
{
	public class HomeController : Controller
	{
		/// <summary>
		/// Used to get the template view for index page. This page does not provide the list of loans.
		/// </summary>
		/// <returns>template view for index page</returns>
		[HttpGet("[action]")]
		public ActionResult Index()
		{
			return View();
		}
	}
}
