using LoanManagementApi.DAL;
using LoanManagementApi.DAL.DAO;
using LoanManagementApi.DAL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
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

			services.AddScoped<ICollateralManagement>(serviceProvider =>
				new CollateralManagement(
					serviceProvider.GetService<IHttpClientFactory>(),
					Configuration.GetValue<string>("CollateralSaveEndpoint"),
					serviceProvider.GetService<ILogger<CollateralManagement>>())
				);
			
			services.AddSwaggerGen(options => options.SwaggerDoc("v1", new OpenApiInfo() { Title = "LoanManagementApi", Version = "v1" }));

			//LoanManagementApi
			services.AddAuthentication(TokenAuthenticationHandler.TokenAuthenticationScheme);
			services
				.AddAuthentication(TokenAuthenticationHandler.TokenAuthenticationScheme)
				.AddScheme<TokenAuthenticationHandlerOptions, TokenAuthenticationHandler>(
					TokenAuthenticationHandler.TokenAuthenticationScheme,
					options => options.Authority = Configuration.GetValue<string>("TokenValidationUrl")
				);
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "LoanManagementApi"));
			}

			app.UseRouting();

			app.UseAuthentication();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
