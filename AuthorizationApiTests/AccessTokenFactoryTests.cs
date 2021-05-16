using AuthorizationApi.DAL.DAO;
using AuthorizationApi.Models;
using AuthorizationApi.Services;
using Microsoft.IdentityModel.Tokens;
using Moq;
using NUnit.Framework;
using System;
using System.Text;

namespace AuthorizationApiTests
{
	[TestFixture]
	public class AccessTokenFactoryTests
	{
		private AccessTokenFactory accessTokenFactory;

		[OneTimeSetUp]
		public void SetupTests()
		{
			SecurityParameters securityParams = new SecurityParameters("mysignveryverylongsecuritykeysothatIdontgetexception", SecurityAlgorithms.HmacSha256, SecurityAlgorithms.HmacSha256Signature);

			Mock<IUserTokensDao> userTokenDao = new Mock<IUserTokensDao>();

			accessTokenFactory = new AccessTokenFactory(securityParams, 300, userTokenDao.Object);
		}

		[Test]
		public void Generate_NullUserId_ShouldThrowArgumentNullException()
		{
			Assert.Throws<ArgumentNullException>(() => accessTokenFactory.Generate(null));
		}

		[Test]
		public void Generate_ValidUserId_ShouldReturnNonNull()
		{
			Assert.IsNotNull(accessTokenFactory.Generate("ranjeet"));
		}
	}
}
