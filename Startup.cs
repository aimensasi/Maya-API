using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Maya.Models;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Validation;
using AspNet.Security.OpenIdConnect.Primitives;
using Maya.Middlewares;

using Maya.Services.UserServices;
using Maya.Services.ProductServices;

namespace Maya {
	public class Startup {
		public Startup(IConfiguration configuration) {
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services) {
			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

			services.AddDbContext<BundleContext>(options => {
				options.UseSqlServer(Configuration.GetConnectionString("BundleContext"));
				options.UseOpenIddict<Guid>();
			});

			ConfigureAppServices(services);

			// Add OpenIddict services
			services.AddOpenIddict().AddCore(options => {
				options.UseEntityFrameworkCore().UseDbContext<BundleContext>().ReplaceDefaultEntities<Guid>();
			}).AddServer(options => {
				options.UseMvc();
				options.EnableTokenEndpoint("/api/oauth/token");

				options.AllowPasswordFlow();
				options.AcceptAnonymousClients();
			}).AddValidation();

			// ASP.NET Core Identity should use the same claim names as OpenIddict
			services.Configure<IdentityOptions>(options => {
				options.ClaimsIdentity.UserNameClaimType = OpenIdConnectConstants.Claims.Name;
				options.ClaimsIdentity.UserIdClaimType = OpenIdConnectConstants.Claims.Subject;
				options.ClaimsIdentity.RoleClaimType = OpenIdConnectConstants.Claims.Role;
			});

			services.AddAuthentication(options => {
				options.DefaultScheme = OpenIddictValidationDefaults.AuthenticationScheme;
			});
			
			AddIdentityCoreServices(services);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env) {

			if (env.IsDevelopment()) {
				app.UseDeveloperExceptionPage();
			} else {
				app.UseHsts();
			}

			// app.UseAdminRegisterBlocker();
			app.UseAuthentication();
			app.UseHttpsRedirection();
			app.UseMvc();
		}

		public void ConfigureAppServices(IServiceCollection services){
			services.AddScoped<IUserServices, UserServices>();
			services.AddScoped<IProductServices, ProductServices>();
		}
		public static void AddIdentityCoreServices(IServiceCollection services) {
			var builder = services.AddIdentityCore<User>(option => {
				option.Password.RequireNonAlphanumeric = false;
				option.Password.RequiredLength = 6;
				option.Password.RequireLowercase = false;
				option.Password.RequireUppercase = false;
				option.Password.RequireDigit = false;
			});
			builder = new IdentityBuilder(builder.UserType, typeof(UserRole), builder.Services);

			builder.AddRoles<UserRole>()
						 .AddEntityFrameworkStores<BundleContext>()
						 .AddDefaultTokenProviders()
						 .AddSignInManager<SignInManager<User>>();
		}

	}
}
