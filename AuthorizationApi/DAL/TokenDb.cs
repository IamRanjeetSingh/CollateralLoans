using AuthorizationApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationApi.DAL
{
	public class TokenDb : DbContext
	{
		public TokenDb(DbContextOptions<TokenDb> options) : base(options)
		{ }

		public DbSet<UserTokens> UserTokens { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<UserTokens>().HasKey(ut => ut.UserId);
			modelBuilder.Entity<UserTokens>().Property(ut => ut.UserId).ValueGeneratedNever();
			modelBuilder.Entity<UserTokens>().Property(ut => ut.LastRefreshToken).IsRequired();

			base.OnModelCreating(modelBuilder);
		}
	}
}
