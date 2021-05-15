using AuthorizationApi.DAL;
using AuthorizationApi.DAL.DAO;
using AuthorizationApi.Models;
using AuthorizationApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationApi
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

			services.AddDbContext<TokenDb>(options =>
			{
				options.UseInMemoryDatabase("TokenDb");
				//options.UseSqlServer("Server=DESKTOP-3CET6HL\\SQLEXPRESS;Database=TokenDb;Trusted_Connection=True;");
			});

			services.AddScoped<IAuthenticationHandler, AuthenticationHandler>();

			services.AddScoped<IUserManager>(serviceProvider => new UserManager(serviceProvider.GetService<ILogger<UserManager>>()));

			services.AddScoped<ITokenManager, TokenManager>();

			services.AddScoped<IUserTokensDao, UserTokensEfDao>();

			string signingKey = Configuration.GetValue<string>("TokenConfig:SigningKey");
			string securityAlgorithm = Configuration.GetValue<string>("TokenConfig:SecurityAlgorithm");
			string securityAlgorithmSignature = Configuration.GetValue<string>("TokenConfig:SecurityAlgorithmSignature");
			SecurityParameters securityParams = new SecurityParameters(signingKey, securityAlgorithm, securityAlgorithmSignature);
			int accessTokenExpiresIn = Configuration.GetValue<int>("TokenConfig:AccessTokenExpiresIn");
			int refreshTokenExpiresIn = Configuration.GetValue<int>("TokenConfig:RefreshTokenExpiresIn");
			services.AddScoped<IAccessTokenFactory>(serviceProvider => new AccessTokenFactory(securityParams, accessTokenExpiresIn, serviceProvider.GetService<IUserTokensDao>()));
			services.AddScoped<IRefreshTokenFactory>(serviceProvider => new RefreshTokenFactory(securityParams, refreshTokenExpiresIn, serviceProvider.GetService<IUserTokensDao>()));

			services.AddSwaggerGen();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			app.UseSwagger();
			app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AuthorizationApi"));

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
