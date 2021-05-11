using LoanManagementApi.DAL;
using LoanManagementApi.DAL.DAO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;

namespace LoanManagementApi
{
	public class Startup
	{
		public IConfiguration Configuration { get; }

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers();

			services.AddHttpClient();

			services.AddDbContext<LoanDb>(options =>
			{
				options.UseInMemoryDatabase("LoanDb");
				//options.UseSqlServer("Server=DESKTOP-3CET6HL\\SQLEXPRESS;Database=LoanDb;Trusted_Connection=True;");
			});

			services.AddScoped<ILoanDao>(serviceProvider =>
				new LoanEfDao(
					serviceProvider.GetService<ILogger<LoanEfDao>>(),
					Configuration.GetSection("LogSensitiveData").Get<bool>()
				));

			services.AddScoped<ICollateralDao>(serviceProvider =>
				new CollateralDao(serviceProvider.GetService<IHttpClientFactory>(), new Uri(Configuration.GetValue<string>("CollateralSaveEndpoint")))
				);
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
