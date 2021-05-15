using CollateralManagementApi.Controllers;
using CollateralManagementApi.DAL;
using CollateralManagementApi.DAL.DAO;
using CollateralManagementApi.Models;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollateralManagementApiTests
{
	[TestFixture]
	class CollateralControllerTests
	{
		private CollateralController controller;
		private Mock<ICollateralDao> daoMock;

		[OneTimeSetUp]
		public void SetupTests()
		{
			daoMock = new Mock<ICollateralDao>();
			controller = new CollateralController(daoMock.Object);
		}


	}
}
