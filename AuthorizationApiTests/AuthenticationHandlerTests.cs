using AuthorizationApi.DAL.DAO;
using AuthorizationApi.DTO;
using AuthorizationApi.Models;
using AuthorizationApi.Services;
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
	public class AuthenticationHandlerTests
	{
		private AuthenticationHandler authHandler;

		[OneTimeSetUp]
		public void SetupTests()
		{
			Mock<IUserManager> userManager = new Mock<IUserManager>();
			userManager.Setup(u => u.ValidateCredentials("ranjeet", "password")).Returns(Task.FromResult(true));

			Token accessToken = new Token("token", DateTime.UtcNow);
			Token refreshToken = new Token("token", DateTime.UtcNow);

			Mock<ITokenManager> tokenManager = new Mock<ITokenManager>();
			tokenManager.Setup(t => t.GenerateNewTokens("ranjeet")).Returns(new TokenContainer(accessToken, refreshToken));
			

			authHandler = new AuthenticationHandler(userManager.Object, tokenManager.Object, null);
		}

		[Test]
		public void AuthenticateAsync_NullRequest_ShouldThrowArgumentNullException()
		{
			Assert.ThrowsAsync<ArgumentNullException>(() => authHandler.AuthenticateAsync(null));
		}

		[Test]
		public void AuthenticateAsync_NullUserId_ShouldThrowArgumentNullException()
		{
			AuthenticationRequest request = new AuthenticationRequest() { UserId = null, Password = "password" };
			Assert.ThrowsAsync<ArgumentException>(() => authHandler.AuthenticateAsync(request));
		}

		[Test]
		public void AuthenticateAsync_NullPassword_ShouldThrowArgumentNullException()
		{
			AuthenticationRequest request = new AuthenticationRequest() { UserId = "ranjeet", Password = null };
			Assert.ThrowsAsync<ArgumentException>(() => authHandler.AuthenticateAsync(request));
		}
	}
}
