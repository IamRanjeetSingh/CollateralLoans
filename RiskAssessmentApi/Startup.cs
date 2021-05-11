using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RiskAssessmentApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace RiskAssessmentApi
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

			services.AddScoped<ILoanManagement>(serviceProvider =>
				new LoanManagement(serviceProvider.GetService<IHttpClientFactory>(), Configuration.GetValue<string>("ApiBaseUrls:LoanManagementApi")));

			services.AddScoped<ICollateralManagement>(serviceProvider =>
				new CollateralManagement(serviceProvider.GetService<IHttpClientFactory>(), Configuration.GetValue<string>("ApiBaseUrls:CollateralManagementApi")));

			services.AddScoped<IRiskAssessment>(serviceProvider =>
				new RiskAssessment(serviceProvider.GetService<ILoanManagement>(), serviceProvider.GetService<ICollateralManagement>()));
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
