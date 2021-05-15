using NUnit.Framework;
using System;
using System.Text.Json;

namespace CollateralManagementApiTests
{
	public static class ObjectExtentions
	{
		public static string AsJson(this object o)
		{
			return JsonSerializer.Serialize(o);
		}
	}
}
