using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationApi.Exceptions
{
	public class InvalidTokenException : SecurityTokenException
	{
		public InvalidTokenException() : base()
		{ }

		public InvalidTokenException(string message) : base(message)
		{ }
	}
}
