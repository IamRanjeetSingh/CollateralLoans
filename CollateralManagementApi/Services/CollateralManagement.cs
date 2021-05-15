using CollateralManagementApi.DAL;
using CollateralManagementApi.DAL.DAO;
using CollateralManagementApi.Models;
using CollateralManagementApi.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace CollateralManagementApi.Services
{
	public class CollateralManagement : ICollateralManagement
	{
		private ICollateralDao _dao;
		private CollateralDb _db;
		private ICollateralDeserializer _collateralDeserializer;

		public static int DefaultPageNo { get => 1; }
		public static int DefaultPageSize { get => 10; }
		public static string DefaultSortProperty { get => CollateralSortProperties.Id; }

		public CollateralManagement(ICollateralDao dao, ICollateralDeserializer collateralSerializer, CollateralDb db)
		{
			_dao = dao;
			_collateralDeserializer = collateralSerializer;
			_db = db;
		}

		public List<Collateral> GetAll(Page page, Filter filter)
		{
			if (page == null)
				page = new Page() { PageNo = DefaultPageNo, PageSize = DefaultPageSize };
			if (page.PageNo <= 0)
				page.PageNo = DefaultPageNo;
			if (page.PageSize <= 0 || page.PageSize > 100)
				page.PageSize = DefaultPageSize;

			if (filter == null)
				filter = new Filter() { SortBy = DefaultSortProperty };
			if (filter.SortBy == null)
				filter.SortBy = DefaultSortProperty;

			return _dao.GetAll(page, filter, _db);
		}

		public Collateral GetById(int id)
		{
			if (id <= 0)
				throw new ArgumentException($"{(id < 0 ? "id is less than 0. " : "")}{(id == 0 ? "id is 0. " : "")}");
			return _dao.GetById(id, _db);
		}

		public int Save(JsonElement collateralJson)
		{
			if (collateralJson.ValueKind != JsonValueKind.Object) 
				throw new ArgumentException("invalid collateral json value kind");
			
			Collateral collateral = _collateralDeserializer.DeserializeByType(collateralJson, nameof(Collateral.Type));
			return _dao.Save(collateral, _db);
		}

		public List<int> SaveMultiple(JsonElement collateralArrayJson)
		{
			if(collateralArrayJson.ValueKind != JsonValueKind.Array)
				throw new ArgumentException("invalid collateral array json value kind");

			List<Collateral> collateralArray = new List<Collateral>();
			for(int index = 0; index < collateralArrayJson.GetArrayLength(); index++)
			{
				try { collateralArray.Add(_collateralDeserializer.DeserializeByType(collateralArrayJson[index], nameof(Collateral.Type))); }
				catch(ArgumentException) { throw new ArgumentException($"invalid collateral json at index {index}"); }
			}

			List<int> collateralsSaveStatuses = new List<int>();
			foreach (Collateral collateral in collateralArray)
				collateralsSaveStatuses.Add(_dao.Save(collateral, _db));

			return collateralsSaveStatuses;
		}
	}
}
