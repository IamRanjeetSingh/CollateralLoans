using CollateralLoanMVC.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CollateralLoanMVC.Controllers
{
	[Route("[controller]")]
	public class HomeController : Controller
	{
		private ICollateralManagement _collateralManagement;

		public HomeController(ICollateralManagement collateralManagement)
		{
			_collateralManagement = collateralManagement;
		}


		/// <summary>
		/// Get the template view for index page. This page does not provide the list of loans.
		/// </summary>
		/// <returns>template view for index page</returns>
		[HttpGet("[action]")]
		public ActionResult Index()
		{
			return View();
		}

		
		//TODO: Remove Debug method
		[HttpGet("{id}")]
		public async Task<IActionResult> Collaterals([FromRoute] int id)
		{
			return Ok(new { list = await _collateralManagement.GetByLoanId(id) });
		}

		//TODO: Remove Debug method
		[HttpGet("[action]/{id}")]
		public async Task<IActionResult> Collateral([FromRoute] int id)
		{
			return Ok(new { collateral = await _collateralManagement.GetCollateral(id) });
		}
	}
}
