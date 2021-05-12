using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationApi.Models
{
	public class Token
	{
		/// <summary>
		/// string representation of this token
		/// </summary>
		public string AsString { get; }

		/// <summary>
		/// time up till which this token is valid
		/// </summary>
		public DateTime ExpiresIn { get; }

		/// <summary>
		/// Initializes a new instance of <see cref="Token"/> class
		/// </summary>
		/// <param name="asString">string representation of this token</param>
		/// <param name="expiresIn">time up till which this token is valid</param>
		public Token(string asString, DateTime expiresIn)
		{
			AsString = asString;
			ExpiresIn = expiresIn;
		}
	}
}
