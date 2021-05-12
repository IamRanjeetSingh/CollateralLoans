using LoanManagementApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoanManagementApi.DAL
{
	public class LoanDb : DbContext
	{
		public LoanDb(DbContextOptions<LoanDb> options) : base(options)
		{ }

		public DbSet<Loan> Loans { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			ConfigureLoanEntity(modelBuilder.Entity<Loan>());
			base.OnModelCreating(modelBuilder);
		}

		private void ConfigureLoanEntity(EntityTypeBuilder<Loan> loanEntity)
		{
			loanEntity.ToTable("Loan");
			loanEntity.HasKey(l => l.Id);
			loanEntity.Property(l => l.Id).ValueGeneratedNever();
			loanEntity.Property(l => l.CustomerId).IsRequired();
			loanEntity.Property(l => l.Type).IsRequired();
			loanEntity.Property(l => l.Principal).IsRequired();
			loanEntity.Property(l => l.Interest).IsRequired();
			loanEntity.Property(l => l.Emi).IsRequired();
			loanEntity.Property(l => l.SanctionDate).IsRequired();
			loanEntity.Property(l => l.MaturityDate).IsRequired();
			//loanEntity.Ignore(l => l.Tenure);
		}

		public void Seed()
		{
			Loans.Add(
				new Loan()
				{
					Id = 1001,
					CustomerId = 2001,
					Type = LoanType.Home,
					Principal = 10_00_000,
					Interest = 0.07,
					Emi = 10_000,
					SanctionDate = DateTime.Now.AddDays(-5),
					MaturityDate = DateTime.Now.AddYears(10)
				}
			);
			Loans.Add(
				new Loan()
				{
					Id = 1002,
					CustomerId = 2001,
					Type = LoanType.Vehicle,
					Principal = 10_00_000,
					Interest = 0.07,
					Emi = 10_000,
					SanctionDate = DateTime.Now.AddDays(-5),
					MaturityDate = DateTime.Now.AddYears(10)
				}
			);
			Loans.Add(
				new Loan()
				{
					Id = 1003,
					CustomerId = 2002,
					Type = LoanType.Home,
					Principal = 10_00_000,
					Interest = 0.07,
					Emi = 10_000,
					SanctionDate = DateTime.Now.AddDays(-5),
					MaturityDate = DateTime.Now.AddYears(10)
				}
			);
			Loans.Add(
				new Loan()
				{
					Id = 1004,
					CustomerId = 2002,
					Type = LoanType.Vehicle,
					Principal = 10_00_000,
					Interest = 0.07,
					Emi = 10_000,
					SanctionDate = DateTime.Now.AddDays(-5),
					MaturityDate = DateTime.Now.AddYears(10)
				}
			);
			SaveChanges();
		}
	}
}
