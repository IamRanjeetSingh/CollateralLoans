using CollateralManagementApi.DAL;
using CollateralManagementApi.DAL.DAO;
using CollateralManagementApi.Models;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace CollateralManagementApiTests
{
	[TestFixture]
	class CollateralEfDaoTests
	{
		private CollateralDb db;
		private CollateralEfDao dao;
		private int DummyCollateralsCount = 100;

		[OneTimeSetUp]
		public void SetupTests()
		{
			dao = new CollateralEfDao();

			DbContextOptionsBuilder<CollateralDb> optionsBuilder = new DbContextOptionsBuilder<CollateralDb>();
			optionsBuilder.UseInMemoryDatabase("CollateralTestDb");

			db = new CollateralDb(optionsBuilder.Options);

			for (int count = 0; count < DummyCollateralsCount; count++)
			{
				if (count % 2 == 0)
				{
					db.Add(new Land()
					{
						LoanId = 1001 + (count % 5),
						CustomerId = 2001 + (count % 6),
						InitialAssesDate = DateTime.Now.AddDays(-(count % 6)),
						LastAssessDate = DateTime.Now.AddDays(count % 4),
						AreaInSqFt = 1000,
						InitialPricePerSqFt = 1100,
						CurrentPricePerSqFt = 1200
					});
				}
				else
				{
					db.Add(new RealEstate()
				{
					LoanId = 1001 + (count % 5),
					CustomerId = 2001 + (count % 6),
					InitialAssesDate = DateTime.Now.AddDays(-(count % 6)),
					LastAssessDate = DateTime.Now.AddDays(count % 6),
					AreaInSqFt = 1000,
					InitialLandPriceInSqFt = 1100,
					CurrentLandPriceInSqFt = 1200,
					InitialStructurePrice = 12000,
					CurrentStructurePrice = 14000,
					YearBuilt = 1990 + ((count % 4) * (count % 3))
				});
			}
		}
			db.SaveChanges();
		}

		[Test]
		public void GetAll_NullPage_ShouldThrowArgumentNullException()
		{
			Assert.Throws<ArgumentNullException>(() => dao.GetAll(null, new Filter(), db));
		}

		[Test]
		public void GetAll_NullFilter_ShouldThrowArgumentNullException()
		{
			Assert.Throws<ArgumentNullException>(() => dao.GetAll(new Page(), null, db));
		}

		[Test]
		public void GetAll_NullDb_ShouldThrowArgumentNullException()
		{
			Assert.Throws<ArgumentNullException>(() => dao.GetAll(new Page(), new Filter(), null));
		}

		[Test]
		public void GetAll_NegativePageNo_ShouldThrowArgumentException()
		{
			Assert.Throws<ArgumentException>(() => dao.GetAll(new Page() { PageNo = 0, PageSize = 5 }, new Filter(), db));
		}

		[Test]
		public void GetAll_ZeroPageNo_ShouldThrowArgumentException()
		{
			Assert.Throws<ArgumentException>(() => dao.GetAll(new Page() { PageNo = 0, PageSize = 5 }, new Filter(), db));
		}

		[Test]
		public void GetAll_OnePageNo_ShouldReturnPageOneContent()
		{
			int pageSize = 5;


			List<Collateral> page1UnderTest = dao.GetAll(new Page() { PageNo = 1, PageSize = pageSize }, new Filter(), db);
			List<Collateral> page1Real = db.Collaterals.Take(pageSize).ToList();

			Assert.IsNotNull(page1UnderTest);
			Assert.IsTrue(page1UnderTest.Count > 0);
			for(int index = 0; index < page1UnderTest.Count && index < page1Real.Count; index++)
				Assert.AreEqual(page1Real[index].AsJson(), page1UnderTest[index].AsJson());
		}

		[Test]
		public void GetAll_NegativePageSize_ShouldThrowArgumentException()
		{
			Assert.Throws<ArgumentException>(() => dao.GetAll(new Page() { PageNo = 1, PageSize = -1 }, new Filter(), db));
		}

		[Test]
		public void GetAll_ZeroPageSize_ShouldThrowArgumentException()
		{
			Assert.Throws<ArgumentException>(() => dao.GetAll(new Page() { PageNo = 1, PageSize = 0 }, new Filter(), db));
		}

		[Test]
		public void GetAll_5PageSize_ShouldReturn5Collaterals()
		{
			int pageSize = 5;

			List<Collateral> pageSizeUnderTest = dao.GetAll(new Page() { PageNo = 1, PageSize = pageSize }, new Filter(), db);
			List<Collateral> pageSizeReal = db.Collaterals.Take(pageSize).ToList();

			Assert.AreEqual(pageSizeReal.Count, pageSizeUnderTest.Count);
		}

		[Test]
		public void GetAll_100PageSize_ShouldReturn100Collaterals()
		{
			int pageSize = 100;

			List<Collateral> pageSizeUnderTest = dao.GetAll(new Page() { PageNo = 1, PageSize = pageSize }, new Filter(), db);
			List<Collateral> pageSizeReal = db.Collaterals.Take(pageSize).ToList();
		}

		
		[Test]
		public void GetAll_LoanIdFilter_ShouldOnlyReturnCollateralsWithGivenLoanId()
		{
			int loanId = 1001;

			List<Collateral> collateralsUnderTest = dao.GetAll(new Page() { PageNo = 1, PageSize = 100 }, new Filter() { LoanId = loanId }, db);

			foreach (Collateral collateral in collateralsUnderTest)
				Assert.AreEqual(loanId, collateral.LoanId);
		}

		[Test]
		public void GetAll_CustomerIdFilter_ShouldOnlyReturnCollateralsWithGivenCustomerId()
		{
			int customerId = 2001;

			List<Collateral> collateralsUnderTest = dao.GetAll(new Page() { PageNo = 1, PageSize = 100 }, new Filter() { CustomerId = customerId }, db);

			foreach (Collateral collateral in collateralsUnderTest)
				Assert.AreEqual(customerId, collateral.CustomerId);
		}

		[Test]
		public void GetAll_LandTypeFilter_ShouldOnlyReturnColleteralsWithLandType()
		{
			string type = CollateralType.Land;

			List<Collateral> collateralsUnderTest = dao.GetAll(new Page() { PageNo = 1, PageSize = 100 }, new Filter() { Type = type }, db);
			foreach (Collateral collateral in collateralsUnderTest)
				Assert.AreEqual(type, collateral.Type);
		}

		[Test]
		public void GetAll_RealEstateTypeFilter_ShouldOnlyReturnColleteralsWithRealEstateType()
		{
			string type = CollateralType.RealEstate;

			List<Collateral> collateralsUnderTest = dao.GetAll(new Page() { PageNo = 1, PageSize = 100 }, new Filter() { Type = type }, db);
			foreach (Collateral collateral in collateralsUnderTest)
				Assert.AreEqual(type, collateral.Type);
		}

		[Test]
		public void GetAll_SortyByLoanIdFilter_ShouldReturnCollateralsSortedByLoanId()
		{
			string sortProperty = CollateralSortProperties.LoanId;

			List<Collateral> collateralsUnderTest = dao.GetAll(new Page() { PageNo = 1, PageSize = 100 }, new Filter() { SortBy = sortProperty }, db);

			int lastLoanId = 0;
			foreach (Collateral collateral in collateralsUnderTest)
			{
				Assert.IsTrue(collateral.LoanId >= lastLoanId);
				lastLoanId = collateral.LoanId;
			}
		}

		[Test]
		public void GetAll_SortyByCustomerIdFilter_ShouldReturnCollateralsSortedByCustomerId()
		{
			string sortProperty = CollateralSortProperties.CustomerId;

			List<Collateral> collateralsUnderTest = dao.GetAll(new Page() { PageNo = 1, PageSize = 100 }, new Filter() { SortBy = sortProperty }, db);

			int lastCustomerId = 0;
			foreach (Collateral collateral in collateralsUnderTest)
			{
				Assert.IsTrue(collateral.CustomerId >= lastCustomerId);
				lastCustomerId = collateral.CustomerId;
			}
		}

		[Test]
		public void GetAll_SortyByInitialAssessDateFilter_ShouldReturnCollateralsSortedByInitialAssessDate()
		{
			string sortProperty = CollateralSortProperties.InitialAssessDate;

			List<Collateral> collateralsUnderTest = dao.GetAll(new Page() { PageNo = 1, PageSize = 100 }, new Filter() { SortBy = sortProperty }, db);

			DateTime lastInitialAssessDate = DateTime.MinValue;
			foreach (Collateral collateral in collateralsUnderTest)
			{
				Assert.IsTrue(collateral.InitialAssesDate >= lastInitialAssessDate);
				lastInitialAssessDate = collateral.InitialAssesDate;
			}
		}

		[Test]
		public void GetAll_SortyByLastAssessDateFilter_ShouldReturnCollateralsSortedByLastAssessDate()
		{
			string sortProperty = CollateralSortProperties.LastAssessDate;

			List<Collateral> collateralsUnderTest = dao.GetAll(new Page() { PageNo = 1, PageSize = 100 }, new Filter() { SortBy = sortProperty }, db);

			DateTime lastLastAssessDate = DateTime.MinValue;
			foreach (Collateral collateral in collateralsUnderTest)
			{
				Assert.IsTrue(collateral.LastAssessDate >= lastLastAssessDate);
				lastLastAssessDate = collateral.LastAssessDate;
			}
		}

		[Test]
		public void GetAll_SortByUnknownProperty_ShouldThrowArgumentException()
		{
			Assert.Throws<ArgumentException>(() => dao.GetAll(new Page() { PageNo = 1, PageSize = 100 }, new Filter() { SortBy = "SomeUnknownProperty" }, db));
		}

		[Test]
		public void GetById_NegativeId_ShouldThrowArgumentException()
		{
			Assert.Throws<ArgumentException>(() => dao.GetById(-1, db));
		}

		[Test]
		public void GetById_ZeroId_ShouldThrowArgumentException()
		{
			Assert.Throws<ArgumentException>(() => dao.GetById(0, db));
		}

		[Test]
		public void GetById_NullDb_ShouldThrowArgumentNullException()
		{
			Assert.Throws<ArgumentNullException>(() => dao.GetById(1, null));
		}

		[Test]
		public void GetById_UnavailableId_ShouldReturnNull()
		{
			int id = 9999;

			Collateral collateral = db.Collaterals.SingleOrDefault(c => c.Id == id);
			if (collateral != null)
			{
				db.Collaterals.Remove(collateral);
				db.SaveChanges();
			}

			Assert.IsNull(dao.GetById(9999, db));
		}

		[Test]
		public void GetById_AvailableId_ShouldReturnCollateralWithGivenId()
		{
			int id = 3001;

			Collateral collateralReal = db.Collaterals.SingleOrDefault(c => c.Id == id);
			if(collateralReal == null)
			{
				collateralReal = new Land()
				{
					LoanId = 1001,
					CustomerId = 2001,
					InitialAssesDate = DateTime.Now.AddDays(-5),
					LastAssessDate = DateTime.Now.AddDays(5),
					AreaInSqFt = 1000,
					InitialPricePerSqFt = 1000,
					CurrentPricePerSqFt = 2000
				};
				db.Collaterals.Add(collateralReal);
				db.SaveChanges();
				id = collateralReal.Id;
			}

			Collateral collateralUnderTest = dao.GetById(id, db);

			Assert.AreEqual(collateralReal.AsJson(), collateralUnderTest.AsJson());
		}

		[Test]
		public void Save_NullCollateral_ShouldThrowArgumentNullException()
		{
			Assert.Throws<ArgumentNullException>(() => dao.Save(null, db));
		}

		[Test]
		public void Save_NullDb_ShouldThrowArgumentNullException()
		{
			Assert.Throws<ArgumentNullException>(() => dao.Save(new Land(), null));
		}

		[Test]
		public void Save_ZeroLoanId_ShouldThrowInvalidOperationException()
		{
			Assert.Throws<InvalidOperationException>(() => dao.Save(new Land()
			{
				LoanId = 0,
				CustomerId = 2001,
				InitialAssesDate = DateTime.Now.AddDays(-5),
				LastAssessDate = DateTime.Now.AddDays(5),
				AreaInSqFt = 1000,
				InitialPricePerSqFt = 1000,
				CurrentPricePerSqFt = 2000,
			}, db));
		}

		[Test]
		public void Save_NegativeLoanId_ShouldThrowInvalidOperationException()
		{
			Assert.Throws<InvalidOperationException>(() => dao.Save(new Land()
			{
				LoanId = -1,
				CustomerId = 2001,
				InitialAssesDate = DateTime.Now.AddDays(-5),
				LastAssessDate = DateTime.Now.AddDays(5),
				AreaInSqFt = 1000,
				InitialPricePerSqFt = 1000,
				CurrentPricePerSqFt = 2000,
			}, db));
		}

		[Test]
		public void Save_ZeroCustomerId_ShouldThrowInvalidOperationException()
		{
			Assert.Throws<InvalidOperationException>(() => dao.Save(new Land()
			{
				LoanId = 1001,
				CustomerId = 0,
				InitialAssesDate = DateTime.Now.AddDays(-5),
				LastAssessDate = DateTime.Now.AddDays(5),
				AreaInSqFt = 1000,
				InitialPricePerSqFt = 1000,
				CurrentPricePerSqFt = 2000,
			}, db));
		}

		[Test]
		public void Save_NegativeCustomerId_ShouldThrowInvalidOperationException()
		{
			Assert.Throws<InvalidOperationException>(() => dao.Save(new Land()
			{
				LoanId = 1001,
				CustomerId = -1,
				InitialAssesDate = DateTime.Now.AddDays(-5),
				LastAssessDate = DateTime.Now.AddDays(5),
				AreaInSqFt = 1000,
				InitialPricePerSqFt = 1000,
				CurrentPricePerSqFt = 2000,
			}, db));
		}

		[Test]
		public void Save_ValidCollateral_ShouldReturnIdOfNewCollateral()
		{
			Collateral collateral = new Land()
			{
				LoanId = 1001,
				CustomerId = 2001,
				InitialAssesDate = DateTime.Now.AddDays(-5),
				LastAssessDate = DateTime.Now.AddDays(5),
				AreaInSqFt = 1000,
				InitialPricePerSqFt = 1000,
				CurrentPricePerSqFt = 1000
			};

			int id = dao.Save(collateral, db);
			Assert.IsTrue(id > 0);
			Assert.AreEqual(collateral.AsJson(), db.Collaterals.SingleOrDefault(c => c.Id == id).AsJson());
		}
	}
}
