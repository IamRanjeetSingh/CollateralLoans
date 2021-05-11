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
		public List<Collateral> GetAll(CollateralDb db, Page page, Filter filter)
		{
			if (page == null || page.PageNo < 1 || page.PageSize < 1)
				throw new ArgumentException("invalid page details");

			IQueryable<Collateral> query = db.Collaterals.AsQueryable();
			
			if(filter != null)
			{
				if (filter.LoanId != 0)
					query = query.Where(c => c.LoanId == filter.LoanId);

				if (filter.CustomerId != 0)
					query = query.Where(c => c.CustomerId == filter.CustomerId);

				if(filter.Type != null)
					query = query.Where(c => c.Type.ToLower().Trim() == filter.Type.ToLower().Trim());

				if (filter.SortBy != null)
				{
					if (!filter.SortBy.EndsWith(CollateralOrder.DescKeyword))
						query = query.OrderBy(CollateralOrder.GetOrder(filter.SortBy));
					else
						query = query.OrderByDescending(CollateralOrder.GetOrder(filter.SortBy));
				}
			}

			query = query.Skip((page.PageNo - 1) * page.PageSize).Take(page.PageSize);

			return query.ToList();
		}

		public Collateral GetById(CollateralDb db, int id)
		{
			return db.Collaterals.SingleOrDefault(c => c.Id == id);
		}

		public int Save(Collateral collateral, CollateralDb db)
		{
			if (collateral == null)
				return 0;

			//verify collateral details

			db.Collaterals.Add(collateral);
			db.SaveChanges();
			return collateral.Id;
		}
	}
}
