using LoanManagementApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace LoanManagementApi.DTO
{
	public class LoanWithCollateralSaveRequest
	{
		public Loan Loan { get; set; }
		public JsonElement Collaterals { get; set; }
	}
}
