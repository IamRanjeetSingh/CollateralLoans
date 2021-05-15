using CollateralManagementApi.Models;
using CollateralManagementApi.Extentions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollateralManagementApi.DAL.DAO
{
	public class CollateralEfDao : ICollateralDao
	{
		public List<Collateral> GetAll(Page page, Filter filter, CollateralDb db)
		{
			if (page == null || filter == null || db == null)
				throw new ArgumentNullException($"{(page == null ? "page is null. " : "")}{(filter == null ? "filter is null. " : "")}{(db == null ? "db is null. " : "")}");
			if (page.PageNo <= 0 || page.PageSize <= 0)
				throw new ArgumentException($"{(page.PageNo < 0 ? "page no is less than zero. " : "")}{(page.PageNo == 0 ? "page no is equal to zero. " : "")}{(page.PageSize <= 0 ? "page size is less than or equal to zero. " : "")}");

			IQueryable<Collateral> query = db.Collaterals.AsQueryable();

			if (filter.LoanId != 0)
				query = query.Where(c => c.LoanId == filter.LoanId);
			if (filter.CustomerId != 0)
				query = query.Where(c => c.CustomerId == filter.CustomerId);
			if (filter.Type != null)
				query = query.Where(c => c.Type.ToLower().Trim() == filter.Type.ToLower().Trim());
			if (filter.SortBy != null)
			{
				if (!filter.SortBy.EndsWith(CollateralSortProperties.DescKeyword))
					query = query.OrderBy(CollateralSortProperties.GetOrder(filter.SortBy));
				else
					query = query.OrderByDescending(CollateralSortProperties.GetOrder(filter.SortBy));
			}

			if ((page.PageNo - 1) * page.PageSize >= query.Count())
			{
				int totalRows = query.Count();
				page.PageNo = totalRows / page.PageSize;
				if (totalRows % page.PageSize != 0 || page.PageNo == 0)
					page.PageNo++;
			}

			query = query.Skip((page.PageNo - 1) * page.PageSize).Take(page.PageSize);

			return query.ToList();
		}

		public Collateral GetById(int id, CollateralDb db)
		{
			if (db == null)
				throw new ArgumentNullException("db is null. ");
			if (id <= 0)
				throw new ArgumentException($"{(id < 0 ? "id is less than zero. " : "")}{(id == 0 ? "id is equal to zero. " : "")}");
			return db.Collaterals.SingleOrDefault(c => c.Id == id);
		}

		public int Save(Collateral collateral, CollateralDb db)
		{
			if (collateral == null || db == null)
				throw new ArgumentNullException($"{(collateral == null ? "collateral is null. " : "")}{(db == null ? "db is null. " : "")}");
			if (collateral.LoanId <= 0 || collateral.CustomerId <= 0)
				throw new InvalidOperationException($"{(collateral.LoanId <= 0 ? "LoandId cannot be less than or equal to 0. " : "")}{(collateral.CustomerId <= 0 ? "CustomerId cannot be less than or equal to 0. " : "")}");

			db.Collaterals.Add(collateral);
			db.SaveChanges();
			return collateral.Id;
		}
	}
}
