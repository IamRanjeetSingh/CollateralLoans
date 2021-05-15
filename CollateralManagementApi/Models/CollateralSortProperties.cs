using CollateralManagementApi.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CollateralManagementApi.Models
{
	public static class CollateralSortProperties
	{
		public const string Id = "id";
		public const string LoanId = "loanid";
		public const string CustomerId = "customerid";
		public const string Type = "type";
		public const string InitialAssessDate = "initialassessdate";
		public const string LastAssessDate = "lastassessdate";
		//public const string InitialValue = "initialvalue";
		//public const string CurrentValue = "currentvalue";

		public const string DescKeyword = "desc";

		public static Expression<Func<Collateral, object>> GetOrder(string order)
		{
			order = order.ToLower().Trim();
			if (order.EndsWith(DescKeyword))
				order = order.RemoveSuffix(DescKeyword);

			if (Id == order)
				return c => c.Id;
			else if (LoanId == order)
				return c => c.LoanId;
			else if (CustomerId == order)
				return c => c.CustomerId;
			else if (Type == order)
				return c => c.Type;
			else if (InitialAssessDate == order)
				return c => c.InitialAssesDate;
			else if (LastAssessDate == order)
				return c => c.LastAssessDate;
			//else if (InitialValue == order)
			//	return c => c.InitialValue;
			//else if (CurrentValue == order)
			//	return c => c.CurrentValue;
			else
				throw new ArgumentException("unknown collateral order");
		}
	}
}
