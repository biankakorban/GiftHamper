using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BiankaKorban_DiplomaProject.Models;
using BiankaKorban_DiplomaProject.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;



namespace GrandeGift
{
	public class Startup
	{
		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			//MVC services
			services.AddMvc();

			//Identity services
			services.AddIdentity<IdentityUser, IdentityRole>
				(
				option =>
				{
					option.User.RequireUniqueEmail = true;
					option.Password.RequireDigit = true;
					option.Password.RequiredLength = 8;
					option.Password.RequireLowercase = true;
					option.Password.RequireNonAlphanumeric = true;
					option.Password.RequireUppercase = true;
				}

			).AddEntityFrameworkStores<MyDbContext>();

			services.AddDbContext<MyDbContext>();

			//config access denied
			services.ConfigureApplicationCookie(

				option =>
				{
					option.AccessDeniedPath = "/Account/Denied";
				}
			);

			//my services
			services.AddScoped<IDataService<Customer>, DataService<Customer>>();
			services.AddScoped<IDataService<Address>, DataService<Address>>();
			services.AddScoped<IDataService<HamperProduct>, DataService<HamperProduct>>();
			services.AddScoped<IDataService<Category>, DataService<Category>>();
			services.AddScoped<IDataService<Product>, DataService<Product>>();
			services.AddScoped<IDataService<Hamper>, DataService<Hamper>>();


		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}



			app.UseStaticFiles();
			//app.UseSession();
			app.UseAuthentication(); //Identity Middleware
			app.UseMvcWithDefaultRoute();


			//SeedHelper.Seed(app.ApplicationServices).Wait();
		}
	}
}
