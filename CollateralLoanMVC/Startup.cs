using CollateralLoanMVC.Services;
using CollateralLoanMVC.Util;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CollateralLoanMVC
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllersWithViews();

			services.AddHttpClient();

			services.AddScoped<ILoanManagement>(serviceProvider => 
				new LoanManagement(
					Configuration.GetValue<string>("ApiBaseUrls:LoanManagement"), 
					serviceProvider.GetService<IHttpClientFactory>(),
					serviceProvider.GetService<ILogger<LoanManagement>>())
				);

			services.AddScoped<IRiskAssessment>(serviceProvider => 
				new RiskAssessment(
					Configuration.GetValue<string>("ApiBaseUrls:RiskAssessment"),
					serviceProvider.GetService<IHttpClientFactory>())
				);

			string authApiBaseUrl = Configuration.GetValue<string>("ApiBaseUrls:AuthManagement");
			services.AddScoped<IAuthManagement>(serviceProvider =>
				new AuthManagement(serviceProvider.GetService<IHttpClientFactory>(), authApiBaseUrl, serviceProvider.GetService<ILogger<AuthManagement>>()));

			string collateralApiBaseUrl = Configuration.GetValue<string>("ApiBaseUrls:CollateralManagement");
			services.AddScoped<ICollateralManagement>(serviceProvider =>
				new CollateralManagement(serviceProvider.GetService<IHttpClientFactory>(), collateralApiBaseUrl, serviceProvider.GetService<ILogger<CollateralManagement>>()));

			services
				.AddAuthentication(TokenAuthenticationHandler.TokenAuthenticationScheme)
				.AddScheme<TokenAuthenticationHandlerOptions, TokenAuthenticationHandler>(
					TokenAuthenticationHandler.TokenAuthenticationScheme,
					options =>
					{
						options.Authority = Configuration.GetValue<string>("ApiBaseUrls:AuthManagement");
					}
				);

			services.AddSession(options =>
			{
				options.IdleTimeout = TimeSpan.FromMinutes(30);
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}
			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseSession();

			app.UseRouting();

			app.UseAuthentication();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}");
			});
		}
	}
}
