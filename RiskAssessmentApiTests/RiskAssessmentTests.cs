using Moq;
using NUnit.Framework;
using RiskAssessmentApi.Models;
using RiskAssessmentApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiskAssessmentApiTests
{
	[TestFixture]
	public class RiskAssessmentTests
	{
		private RiskAssessment riskAssessment;

		[OneTimeSetUp]
		public void SetupTests()
		{
			Mock<ICollateralManagement> collateralManagement = new Mock<ICollateralManagement>();
			Mock<ILoanManagement> loanManagement = new Mock<ILoanManagement>();

			riskAssessment = new RiskAssessment(loanManagement.Object, collateralManagement.Object);
		}

		[Test]
		public void EvaluateAsync_NegativeLoanId_ShouldThrowArgumentException()
		{
			Assert.ThrowsAsync<ArgumentException>(() => riskAssessment.EvaluateAsync(-1));
		}

		[Test]
		public void EvaluateAsync_ZeroLoanId_ShouldThrowArgumentException()
		{
			Assert.ThrowsAsync<ArgumentException>(() => riskAssessment.EvaluateAsync(0));
		}


	}
}
