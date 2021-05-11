using CollateralManagementApi.Controllers;
using CollateralManagementApi.DAL;
using CollateralManagementApi.DAL.DAO;
using CollateralManagementApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollateralManagementApi
{
	public class Startup
	{
		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers();

			services.AddDbContext<CollateralDb>(options => 
			{
				options.UseInMemoryDatabase("CollateralDb");
				//options.UseSqlServer("Server=DESKTOP-3CET6HL\\SQLEXPRESS;Database=CollateralDb;Trusted_Connection=True;");
			});

			services.AddScoped<ISubCollateralDao<Land>, LandEfDao>();
			services.AddScoped<ISubCollateralDao<RealEstate>, RealEstateEfDao>();

			services.AddSingleton<ICollateralDao, CollateralEfDao>();

			services.AddSwaggerGen(options => options.SwaggerDoc("v1", new OpenApiInfo() { Title = "CollateralManagementApi", Version = "v1" }));
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "CollateralManagementApi"));
			}

			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
