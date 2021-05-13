using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationApi.Exceptions
{
	public class UnauthenticTokenException : SecurityTokenException
	{
		public UnauthenticTokenException() : base()
		{ }

		public UnauthenticTokenException(string message) : base(message)
		{ }
	}
}
