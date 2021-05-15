using CollateralManagementApi.DAL;
using CollateralManagementApi.DAL.DAO;
using CollateralManagementApi.Models;
using CollateralManagementApi.Services;
using CollateralManagementApi.Util;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace CollateralManagementApiTests
{
	[TestFixture]
	public class CollateralManagementTests
	{
		private CollateralManagement collateralManagement;
		private Mock<ICollateralDeserializer> collateralDeserializer;
		private Mock<ICollateralDao> dao;

		[OneTimeSetUp]
		public void SetupTests()
		{
			dao = new Mock<ICollateralDao>();
			collateralDeserializer = new Mock<ICollateralDeserializer>();

			CollateralDb db = new Mock<CollateralDb>(new DbContextOptionsBuilder<CollateralDb>().Options).Object;
			collateralManagement = new CollateralManagement(dao.Object, collateralDeserializer.Object, db);
		}

		[Test]
		public void GetAll_NullPage_ShouldReturnContentOfDefaultPageNoWithDefaultSize()
		{
			collateralManagement.GetAll(null, new Filter());
			dao.Verify(m =>
				m.GetAll(
					It.Is<Page>(p => p != null && p.PageNo == CollateralManagement.DefaultPageNo && p.PageSize == CollateralManagement.DefaultPageSize),
					It.Is<Filter>(f => f != null && f.SortBy == CollateralManagement.DefaultSortProperty),
					It.Is<CollateralDb>(db => db != null)));
		}

		[Test]
		public void GetAll_NegativePageNo_ShouldReturnPage1Content()
		{
			Page page = new Page() { PageNo = -1, PageSize = 1 };

			collateralManagement.GetAll(page, null);

			dao.Verify(m =>
				m.GetAll(
					It.Is<Page>(p => p != null && p.PageNo == CollateralManagement.DefaultPageNo && p.PageSize == page.PageSize),
					It.Is<Filter>(f => f != null && f.SortBy == CollateralManagement.DefaultSortProperty),
					It.Is<CollateralDb>(db => db != null)));
		}

		[Test]
		public void GetAll_ZeroPageNo_ShouldReturnPage1Content()
		{
			Page page = new Page() { PageNo = 0, PageSize = 1 };

			collateralManagement.GetAll(page, null);

			dao.Verify(m =>
				m.GetAll(
					It.Is<Page>(p => p != null && p.PageNo == CollateralManagement.DefaultPageNo && p.PageSize == page.PageSize),
					It.Is<Filter>(f => f != null && f.SortBy == CollateralManagement.DefaultSortProperty),
					It.Is<CollateralDb>(db => db != null)));
		}

		[Test]
		public void GetAll_NPageNo_ShouldReturnPageNContent()
		{
			Page page = new Page() { PageNo = 5, PageSize = 1 };

			collateralManagement.GetAll(page, null);

			dao.Verify(m =>
				m.GetAll(
					It.Is<Page>(p => p != null && p.PageNo == page.PageNo && p.PageSize == page.PageSize),
					It.Is<Filter>(f => f != null && f.SortBy == CollateralManagement.DefaultSortProperty),
					It.Is<CollateralDb>(db => db != null)));
		}

		[Test]
		public void GetAll_NegativePageSize_ShouldReturnPageOfSize10()
		{
			Page page = new Page() { PageNo = 1, PageSize = -1 };

			collateralManagement.GetAll(page, null);

			dao.Verify(m =>
				m.GetAll(
					It.Is<Page>(p => p != null && p.PageNo == page.PageNo && p.PageSize == CollateralManagement.DefaultPageSize),
					It.Is<Filter>(f => f != null && f.SortBy == CollateralManagement.DefaultSortProperty),
					It.Is<CollateralDb>(db => db != null)));
		}

		[Test]
		public void GetAll_ZeroPageSize_ShouldReturnPageOfSize10()
		{
			Page page = new Page() { PageNo = 1, PageSize = 0 };

			collateralManagement.GetAll(page, null);

			dao.Verify(m =>
				m.GetAll(
					It.Is<Page>(p => p != null && p.PageNo == page.PageNo && p.PageSize == CollateralManagement.DefaultPageSize),
					It.Is<Filter>(f => f != null && f.SortBy == CollateralManagement.DefaultSortProperty),
					It.Is<CollateralDb>(db => db != null)));
		}

		[Test]
		public void GetAll_NullFilter_ShouldReturnListSortedById()
		{
			collateralManagement.GetAll(new Page(), null);

			dao.Verify(m =>
				m.GetAll(
					It.Is<Page>(p => p != null && p.PageNo == CollateralManagement.DefaultPageNo && p.PageSize == CollateralManagement.DefaultPageSize),
					It.Is<Filter>(f => f != null && f.SortBy == CollateralManagement.DefaultSortProperty),
					It.IsAny<CollateralDb>()));
		}

		[Test]
		public void GetAll_FilterWithValidSortProperty_ShouldReturnListFilteredAccordingly()
		{
			Filter filter = new Filter() { SortBy = CollateralSortProperties.CustomerId };
			collateralManagement.GetAll(null, filter);

			dao.Verify(m =>
				m.GetAll(
					It.Is<Page>(p => p != null && p.PageNo == CollateralManagement.DefaultPageNo && p.PageSize == CollateralManagement.DefaultPageSize),
					It.Is<Filter>(f => f != null && f.SortBy == filter.SortBy),
					It.Is<CollateralDb>(db => db != null)));
		}

		[Test]
		public void GetById_NegativeId_ShouldThrowArgumentException()
		{
			Assert.Throws<ArgumentException>(() => collateralManagement.GetById(-1));
		}

		[Test]
		public void GetById_ZeroId_ShouldThrowArgumentException()
		{
			Assert.Throws<ArgumentException>(() => collateralManagement.GetById(0));
		}

		[Test]
		public void GetById_UnavailableId_ShouldReturnNull()
		{
			int unavailableId = 1;

			dao.Setup(m => m.GetById(unavailableId, It.Is<CollateralDb>(db => db != null))).Returns(value: null);

			Assert.IsNull(collateralManagement.GetById(1));
		}

		[Test]
		public void GetById_AvailableId_ShouldReturnAssociatedCollateral()
		{
			int availableId = 1;

			dao.Setup(m => m.GetById(availableId, It.Is<CollateralDb>(db => db != null))).Returns(new Land());

			Assert.IsNotNull(collateralManagement.GetById(1));
		}

		[Test]
		public void Save_InvalidCollateralJson_ShouldThrowArgumentException()
		{
			Assert.Throws<ArgumentException>(() => collateralManagement.Save(new JsonElement()));
		}

		[Test]
		public void Save_ValidCollateralJson_ShouldReturnIdOfNewCollateral()
		{
			Collateral validCollateral = new Land()
			{
				LoanId = 1001,
				CustomerId = 2001,
				InitialAssesDate = DateTime.Now.AddDays(-5),
				LastAssessDate = DateTime.Now.AddDays(5),
				AreaInSqFt = 1000,
				InitialPricePerSqFt = 1100,
				CurrentPricePerSqFt = 1200
			};
			JsonElement validCollateralJson = JsonDocument.Parse(JsonSerializer.Serialize(validCollateral)).RootElement;

			int newCollateralId = 1;
			collateralDeserializer.Setup(m => m.DeserializeByType(It.Is<JsonElement>(je => je.AsJson() == validCollateralJson.AsJson()), nameof(Collateral.Type)))
				.Returns(validCollateral);
			dao.Setup(m => m.Save(It.Is<Collateral>(c => c.AsJson() == validCollateral.AsJson()), It.IsAny<CollateralDb>())).Returns(newCollateralId);

			Assert.That(collateralManagement.Save(validCollateralJson) == newCollateralId);

			dao.Verify(m => m.Save(It.Is<Collateral>(c => c.AsJson() == validCollateral.AsJson()), It.Is<CollateralDb>(db => db != null)));
		}

		[Test]
		public void SaveMultiple_InvalidCollateralArrayJson_ShouldThrowArgumentException()
		{
			Assert.Throws<ArgumentException>(() => collateralManagement.SaveMultiple(new JsonElement()));
		}

		[Test]
		public void SaveMultiple_ValidCollateralArrayJson_ShouldReturnArrayOfNewCollateralIds()
		{
			List<Collateral> validCollateralArray = new List<Collateral>(){
				new Land()
				{
					LoanId = 1001,
					CustomerId = 2001,
					InitialAssesDate = DateTime.Now.AddDays(-5),
					LastAssessDate = DateTime.Now.AddDays(5),
					AreaInSqFt = 1000,
					InitialPricePerSqFt = 1100,
					CurrentPricePerSqFt = 1200
				}
			};
			JsonElement validCollateralArrayJson = JsonDocument.Parse($"{JsonSerializer.Serialize(validCollateralArray)}").RootElement;

			int newCollateralId = 1;
			collateralDeserializer.Setup(m => m.DeserializeByType(It.Is<JsonElement>(je => je.AsJson() == validCollateralArray[0].AsJson()), nameof(Collateral.Type)))
				.Returns(validCollateralArray[0]);
			dao.Setup(m => m.Save(It.IsAny<Collateral>(), It.IsAny<CollateralDb>())).Returns(newCollateralId);

			Assert.That(collateralManagement.SaveMultiple(validCollateralArrayJson)[0] == newCollateralId);

			dao.Verify(m => m.Save(It.Is<Collateral>(c => c.AsJson() == validCollateralArray[0].AsJson()), It.Is<CollateralDb>(db => db != null)));
		}
	}
}
