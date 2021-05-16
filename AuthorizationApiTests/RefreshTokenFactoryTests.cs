using AuthorizationApi.DAL.DAO;
using AuthorizationApi.Models;
using AuthorizationApi.Services;
using Microsoft.IdentityModel.Tokens;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthorizationApiTests
{
	[TestFixture]
	public class RefreshTokenFactoryTests
	{
		private RefreshTokenFactory refreshTokenFactory;

		[OneTimeSetUp]
		public void SetupTests()
		{
			SecurityParameters securityParams = new SecurityParameters("mysignveryverylongsecuritykeysothatIdontgetexception", SecurityAlgorithms.HmacSha256, SecurityAlgorithms.HmacSha256Signature);

			Mock<IUserTokensDao> userTokenDao = new Mock<IUserTokensDao>();

			refreshTokenFactory = new RefreshTokenFactory(securityParams, 300, userTokenDao.Object);
		}

		[Test]
		public void Generate_NullUserId_ShouldThrowArgumentNullException()
		{
			Assert.Throws<ArgumentNullException>(() => refreshTokenFactory.Generate(null));
		}

		[Test]
		public void Generate_ValidUserId_ShouldReturnNonNull()
		{
			Assert.IsNotNull(refreshTokenFactory.Generate("ranjeet"));
		}
	}
}
