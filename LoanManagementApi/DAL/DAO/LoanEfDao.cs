using LoanManagementApi.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LoanManagementApi.DAL.DAO
{
	public class LoanEfDao : ILoanDao
	{
		private ILogger<LoanEfDao> _logger;
		private bool _logSensitiveData;

		public LoanEfDao(ILogger<LoanEfDao> logger, bool logSensitiveData)
		{
			_logger = logger;
			_logSensitiveData = logSensitiveData;
		}

		public List<Loan> GetAll(Page page, Filter filter, LoanDb db)
		{
			page ??= new Page() { PageNo = 1, PageSize = 10 };
			page.PageSize = page.PageSize <= 0 ? 10 : page.PageSize;

			if (db == null) throw new ArgumentNullException($"{typeof(LoanDb).FullName} parameter shouldn't be null");

			IQueryable<Loan> query = db.Loans.AsQueryable();

			if (filter != null)
			{
				if (filter.CustomerId != 0)
					query = query.Where(l => l.CustomerId == filter.CustomerId);
				if (filter.Type != null)
					query = query.Where(l => l.Type == filter.Type);
				if (filter.MinPrincipal > 0)
					query = query.Where(l => l.Principal >= filter.MinPrincipal);
				if (filter.MaxPrincipal > 0)
					query = query.Where(l => l.Principal <= filter.MaxPrincipal);
				if (filter.MinInterest > 0)
					query = query.Where(l => l.Interest >= filter.MinInterest);
				if (filter.MaxInterest > 0)
					query = query.Where(l => l.Interest <= filter.MaxInterest);
				if (filter.MinEmi > 0)
					query = query.Where(l => l.Emi >= filter.MinEmi);
				if (filter.MaxEmi > 0)
					query = query.Where(l => l.Emi <= filter.MaxEmi);
				if (filter.SanctionAfter != DateTime.MinValue)
					query = query.Where(l => l.SanctionDate >= filter.SanctionAfter);
				if (filter.SanctionBefore != DateTime.MinValue)
					query = query.Where(l => l.SanctionDate <= filter.SanctionBefore);
				if (filter.MaturityAfter != DateTime.MinValue)
					query = query.Where(l => l.MaturityDate >= filter.MaturityAfter);
				if (filter.MaturityBefore != DateTime.MinValue)
					query = query.Where(l => l.MaturityDate <= filter.MaturityBefore);
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

		public Loan GetById(int id, LoanDb db)
		{
			if (db == null) throw new ArgumentNullException($"{typeof(LoanDb).FullName} parameter shouldn't be null");

			return db.Loans.SingleOrDefault(l => l.Id == id);
		}

		public int Save(Loan loan, LoanDb db)
		{
			if (IsDbContextNull(db) || IsLoanNull(loan))
				return 0;

			db.Loans.Add(loan);
			try
			{
				db.SaveChanges();
				return loan.Id;
			}
			catch (Exception e)
			{
				_logger.LogError(e, $"Error occurred while saving customer{(_logSensitiveData ? JsonConvert.SerializeObject(loan) : "")}");
				return 0;
			}
		}

		public int UpdateFull(int id, Loan loan, LoanDb db)
		{
			if (IsDbContextNull(db) || IsLoanNull(loan))
				return 0;

			Loan trackedLoan = null;
			try
			{
				trackedLoan = db.Loans.SingleOrDefault(c => c.Id == id);
			}
			catch (Exception e)
			{
				_logger.LogError(e, $"Error occurred while getting loan by id{(_logSensitiveData ? "{" + id + "}" : "")} for full update");
				return 0;
			}

			if (trackedLoan == null)
			{
				_logger.LogDebug($"No loan found for given id{(_logSensitiveData ? "{" + id + "}" : "")}");
				return 0;
			}

			loan.Id = id;   //avoid updating the id
			try
			{
				db.Entry(trackedLoan).CurrentValues.SetValues(loan);
				return db.SaveChanges();
			}
			catch (Exception e)
			{
				_logger.LogError(e, $"Error occurred while fully updating loan{(_logSensitiveData ? JsonConvert.SerializeObject(loan) : "")}");
				return 0;
			}
		}

		public int UpdatePartial(int id, dynamic loan, LoanDb db)
		{
			if (IsDbContextNull(db))
				return 0;

			Loan trackedLoan = null;
			try
			{
				trackedLoan = db.Loans.SingleOrDefault(c => c.Id == id);
			}
			catch (Exception e)
			{
				_logger.LogError(e, $"Error occurred while getting loan by id{(_logSensitiveData ? "{" + id + "}" : "")} for partial update");
			}

			if (trackedLoan == null)
			{
				_logger.LogDebug($"No loan found for given id{(_logSensitiveData ? "{" + id + "}" : "")}");
				return 0;
			}

			if (loan.GetType().GetProperty("Id") != null)
				loan.Id = id; //cannot update the id

			try
			{
				db.Entry(trackedLoan).CurrentValues.SetValues(loan);
				return db.SaveChanges();
			}
			catch (Exception e)
			{
				_logger.LogError(e, $"Error occurred while partially updating loan{(_logSensitiveData ? JsonConvert.SerializeObject(loan, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }) : "")}");
				return 0;
			}
		}

		public int Delete(int id, LoanDb db)
		{
			if (IsDbContextNull(db))
				return 0;

			Loan trackedLoan = null;
			try
			{
				trackedLoan = db.Loans.SingleOrDefault(c => c.Id == id);
			}
			catch (Exception e)
			{
				_logger.LogError(e, $"Error occurred while getting loan by id{(_logSensitiveData ? "{" + id + "}" : "")} for delete");
			}

			if (trackedLoan == null)
			{
				_logger.LogDebug($"No loan found for given id{(_logSensitiveData ? "{" + id + "}" : "")}");
				return 0;
			}

			try
			{
				db.Loans.Remove(trackedLoan);
				return db.SaveChanges();
			}
			catch (Exception e)
			{
				_logger.LogError(e, $"Error occurred while deleting loan by id{(_logSensitiveData ? "{" + id + "}" : "")}");
				return 0;
			}
		}

		private bool IsDbContextNull(LoanDb db)
		{
			if (db == null)
			{
				_logger.LogError(new ArgumentNullException($"{nameof(LoanDb)} instance is required to perform database operations"), $"{nameof(LoanDb)} is null");
				return true;
			}
			return false;
		}

		private bool IsLoanNull(Loan loan)
		{
			if (loan == null)
			{
				_logger.LogError(new ArgumentNullException($"{nameof(Loan)} is null, so it cannot be used for database operation"), $"{nameof(Loan)} is null");
				return true;
			}
			return false;
		}
	}
}
