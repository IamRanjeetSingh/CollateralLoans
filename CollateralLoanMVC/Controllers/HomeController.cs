using CollateralLoanMVC.Exceptions;
using CollateralLoanMVC.Models;
using CollateralLoanMVC.Services;
using CollateralLoanMVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace CollateralLoanMVC.Controllers
{
	[Authorize]
	[Route("")]
	[Route("[controller]")]
	public class HomeController : Controller
	{
		private ICollateralManagement _collateralManagement;
		private ILogger<HomeController> _logger;

		public HomeController(ICollateralManagement collateralManagement, ILogger<HomeController> logger)
		{
			_collateralManagement = collateralManagement;
			_logger = logger;
		}


		[Route("")]
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
			var collateralJson = await _collateralManagement.GetCollateral(id);
			_logger.LogInformation(collateralJson.GetRawText());

			JsonElement type;
			if (!collateralJson.TryGetProperty(nameof(Models.Collateral.Type).Trim(), out type) && !collateralJson.TryGetProperty(nameof(Models.Collateral.Type).ToLower().Trim(), out type))
				throw new UnexpectedResponseException("no type property found");

			string collateralType = type.GetString();

			JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
			if (collateralType.ToLower() == nameof(Land).ToLower())
			{
				Land land = JsonSerializer.Deserialize<Land>(collateralJson.GetRawText(), jsonSerializerOptions);
				return View("View", new ViewCollateralViewModel { Land = land });
				//return Ok(new { collateral = land });
			}
			else
			{
				RealEstate realEstate = JsonSerializer.Deserialize<RealEstate>(collateralJson.GetRawText(), jsonSerializerOptions);
				return View("View", new ViewCollateralViewModel { RealEstate = realEstate });
				//return Ok(new { collateral = realEstate });
			}
		}
	}
}
