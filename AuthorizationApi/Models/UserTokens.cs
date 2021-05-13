using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationApi.Models
{
	public class UserTokens
	{
		public string UserId { get; set; }
		public string RefreshToken { get; set; }
	}
}
