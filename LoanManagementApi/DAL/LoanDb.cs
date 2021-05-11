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
	}
}
