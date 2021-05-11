using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RiskAssessmentApi.Exceptions
{
	public class UnexpectedResponseException : SystemException
	{
		public UnexpectedResponseException()
		{ }

		public UnexpectedResponseException(string message) : base(message)
		{ }

		public UnexpectedResponseException(string message, Exception innerException) : base(message, innerException)
		{ }
	}
}
