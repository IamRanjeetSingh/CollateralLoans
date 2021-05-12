using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationApi.Models
{
	public class SecurityParameters
	{
		public SecurityKey SigningKey { get; }
		public string SecurityAlgorithm { get; }
		public string SecurityAlgorithmSignature { get; }

		public SecurityParameters(SecurityKey signingKey, string securityAlgorithm, string securityAlgorithmSignature)
		{
			SigningKey = signingKey;
			SecurityAlgorithm = securityAlgorithm;
			SecurityAlgorithmSignature = securityAlgorithmSignature;
		}
	}
}
