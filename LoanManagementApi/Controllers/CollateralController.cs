using LoanManagementApi.DAL.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace LoanManagementApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CollateralController : ControllerBase
	{
		private ICollateralManagement _collateralManagement;

		public CollateralController(ICollateralManagement collateralManagement)
		{
			_collateralManagement = collateralManagement;
		}

		[HttpPost("")]
		public async Task<IActionResult> SaveCollaterals([FromBody] JsonElement collateralsJson)
		{
			if (collateralsJson.ValueKind != JsonValueKind.Array)
				return BadRequest(new { error = "Invalid Collateral Array" });

			HttpResponseMessage response;
			try { response = await _collateralManagement.Save(collateralsJson); }
			catch (HttpRequestException) { return StatusCode((int)HttpStatusCode.ServiceUnavailable); }

			if (response.StatusCode != HttpStatusCode.OK)
				return StatusCode((int)response.StatusCode);

			JsonElement responseBody = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;
			return Ok(responseBody);
		}
	}
}
