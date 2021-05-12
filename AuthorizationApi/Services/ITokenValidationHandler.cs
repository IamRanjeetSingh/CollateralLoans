using AuthorizationApi.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationApi.Services
{
	/// <summary>
	/// Responsible for handling token validation.
	/// </summary>
	public interface ITokenValidationHandler
	{
		TokenValidationResponse Validate(TokenValidationRequest request);
	}
}
