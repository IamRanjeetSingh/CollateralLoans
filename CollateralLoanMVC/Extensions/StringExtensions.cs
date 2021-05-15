using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollateralLoanMVC.Extentions
{
	public static class StringExtensions
	{
		public static string RemoveSuffix(this string s, string suffix)
		{
			if (suffix == null || !s.EndsWith(suffix))
				return s;
			return s.Remove(s.Length - suffix.Length);
		}
	}
}
