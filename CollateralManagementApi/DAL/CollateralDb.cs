using CollateralManagementApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollateralManagementApi.DAL
{
	public class CollateralDb : DbContext
	{
		public CollateralDb(DbContextOptions<CollateralDb> options) : base(options)
		{ }

		public DbSet<Collateral> Collaterals { get; set; }
		public DbSet<Land> Lands { get; set; }
		public DbSet<RealEstate> RealEstates { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			ConfigureCollaterals(modelBuilder.Entity<Collateral>());
			ConfigureLands(modelBuilder.Entity<Land>());
			ConfigureRealEstates(modelBuilder.Entity<RealEstate>());

			base.OnModelCreating(modelBuilder);
		}

		private void ConfigureCollaterals(EntityTypeBuilder<Collateral> collateralEntity)
		{
			collateralEntity.ToTable("Collateral");
			collateralEntity.HasKey(c => c.Id);
			collateralEntity.Property(c => c.Id).ValueGeneratedNever();

			collateralEntity.Property(c => c.InitialValue).IsRequired();
			collateralEntity.Property(c => c.CurrentValue).IsRequired();
		}

		private void ConfigureLands(EntityTypeBuilder<Land> landEntity)
		{
			landEntity.ToTable("Land");
			landEntity
				.HasOne<Collateral>()
				.WithOne()
				.HasForeignKey<Land>(l => l.Id);
		}

		private void ConfigureRealEstates(EntityTypeBuilder<RealEstate> realEstateEntity)
		{
			realEstateEntity.ToTable("RealEstate");
			realEstateEntity
				.HasOne<Collateral>()
				.WithOne()
				.HasForeignKey<RealEstate>(r => r.Id);
		}

		public void SeedData()
		{
			Collaterals.Add(
				new Land()
				{
					Id = 3001,
					LoanId = 1001,
					CustomerId = 2001,
					InitialAssesDate = DateTime.Now.AddDays(-5),
					LastAssessDate = DateTime.Now,
					AreaInSqFt = 1000,
					InitialPricePerSqFt = 1000,
					CurrentPricePerSqFt = 2000
				});
			Collaterals.Add(
				new RealEstate()
				{
					Id = 3002,
					LoanId = 1001,
					CustomerId = 2001,
					InitialAssesDate = DateTime.Now.AddDays(-6),
					LastAssessDate = DateTime.Now,
					AreaInSqFt = 1100,
					InitialLandPriceInSqFt = 1000,
					CurrentLandPriceInSqFt = 2000,
					InitialStructurePrice = 100000,
					CurrentStructurePrice = 200000,
					YearBuilt = 2004
				});
			Collaterals.Add(
				new Land()
				{
					Id = 3003,
					LoanId = 1002,
					CustomerId = 2001,
					InitialAssesDate = DateTime.Now.AddDays(-7),
					LastAssessDate = DateTime.Now,
					AreaInSqFt = 1200,
					InitialPricePerSqFt = 1000,
					CurrentPricePerSqFt = 2000
				});
			Collaterals.Add(
				new RealEstate()
				{
					Id = 3004,
					LoanId = 1003,
					CustomerId = 2002,
					InitialAssesDate = DateTime.Now.AddDays(-8),
					LastAssessDate = DateTime.Now,
					AreaInSqFt = 1300,
					InitialLandPriceInSqFt = 1000,
					CurrentLandPriceInSqFt = 2000,
					InitialStructurePrice = 100000,
					CurrentStructurePrice = 200000,
					YearBuilt = 2004
				});
			SaveChanges();
		}
	}
}
