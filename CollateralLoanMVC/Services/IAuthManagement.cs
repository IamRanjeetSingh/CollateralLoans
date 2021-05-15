using CollateralLoanMVC.DTO;
using CollateralLoanMVC.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CollateralLoanMVC.Services
{
	public interface IAuthManagement
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException">userid or password is null or empty</exception>
		/// <exception cref="HttpRequestException">unable to connect with authorization api</exception>
		/// <exception cref="UnexpectedResponseException">something went wrong in AuthorizationApi</exception>
		Task<AuthenticationResponse> Login(string userId, string password);
	}
}
