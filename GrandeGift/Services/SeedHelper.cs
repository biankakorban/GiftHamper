using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//
using BiankaKorban_DiplomaProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace BiankaKorban_DiplomaProject.Services
{
        public static class SeedHelper
        {
            public static async Task Seed(IServiceProvider provider)
            {
                var scopeFactory = provider.GetRequiredService<IServiceScopeFactory>();
                using (var scope = scopeFactory.CreateScope())
                {
                    UserManager<IdentityUser> userManger = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                    RoleManager<IdentityRole> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                    DataService<Customer> customerService = new DataService<Customer>();
                    DataService<Admin> adminService = new DataService<Admin>();

                //add customer role
                if (await roleManager.FindByNameAsync("Customer") == null)
                    {
                        await roleManager.CreateAsync(new IdentityRole("Customer"));
                    }
                    //add default customer
                    if (await userManger.FindByNameAsync("customer") == null)
                    {
                        IdentityUser customer = new IdentityUser("customer");
                        customer.Email = "customer@yahoo.com";
                        await userManger.CreateAsync(customer, "Apple3###");
                        //add profile
                        Customer customerProfile = new Customer
                        {
                            FirstName = "customer",
                            LastName = "Default",
                            UserId = customer.Id,
							UserName = customer.UserName,
							Email = "",
							Phone = ""
                        };
                        customerService.Create(customerProfile);
                        //add this default customer to Customer role
                        await userManger.AddToRoleAsync(customer, "Customer");
                    }

                    //add admin role
                    if (await roleManager.FindByNameAsync("Admin") == null)
                    {
                        await roleManager.CreateAsync(new IdentityRole("Admin"));
                    }
                    //add default admin
                    if (await userManger.FindByNameAsync("admin") == null)
                    {
                        IdentityUser admin = new IdentityUser("admin");
                        admin.Email = "admin@yahoo.com";
                        await userManger.CreateAsync(admin, "Apple3###");
                        //add profile
                        Admin adminProfile = new Admin
                        {
                            FirstName = "Adminstrator",
                            LastName = "Default",
                            UserId = admin.Id,
							UserName = admin.UserName,
							Email = "",
							Phone = ""
                        };
                        adminService.Create(adminProfile);
                        //add this default admin to Admin role
                        await userManger.AddToRoleAsync(admin, "Admin");
                    }

                }

            }
        }
}
