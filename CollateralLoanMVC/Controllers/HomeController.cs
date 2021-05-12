using Microsoft.AspNetCore.Mvc;

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
	}
}
