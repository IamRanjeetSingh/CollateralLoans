using LoanManagementApi.DAL.DAO;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanManagementApiTests
{
	[TestFixture]
	public class LoanEfDaoTests
	{
		private LoanEfDao dao;

		[OneTimeSetUp]
		public void SetupTests()
		{
			dao = new LoanEfDao(null, false);
		}

		[Test]
		public void GetById_NegativeId_ShouldThrowArgumentException()
		{
			Assert.Throws<ArgumentException>(() => dao.GetById(-1, null));
		}

		[Test]
		public void GetById_ZeroId_ShouldThrowArgumentException()
		{
			Assert.Throws<ArgumentException>(() => dao.GetById(0, null));
		}

		[Test]
		public void GetById_NullDb_ShouldThrowArgumentNullException()
		{
			Assert.Throws<ArgumentNullException>(() => dao.GetById(1, null));
		}

		[Test]
		public void Save_NullLoan_ShouldThrowArgumentNullException()
		{
			Assert.Throws<ArgumentNullException>(() => dao.Save(null, null));
		}

		[Test]
		public void Save_NullDb_ShouldThrowArgumentNullException()
		{
			Assert.Throws<ArgumentNullException>(() => dao.Save(null, null));
		}
	}
}
