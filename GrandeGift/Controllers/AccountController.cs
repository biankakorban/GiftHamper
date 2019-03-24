using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
//
using BiankaKorban_DiplomaProject.Services;
using BiankaKorban_DiplomaProject.Models;
using Microsoft.AspNetCore.Identity;
using BiankaKorban_DiplomaProject.ViewModels;

namespace BiankaKorban_DiplomaProject.Controllers
{
    public class AccountController : Controller
    {

        private IDataService<Customer> _customerDataService;
        private UserManager<IdentityUser> _userManagerService;
        private SignInManager<IdentityUser> _signInManagerService;
        private RoleManager<IdentityRole> _roleManagerService;

        public AccountController(IDataService<Customer> customerDataService, 
                                 UserManager<IdentityUser> userManagerService, 
                                 SignInManager<IdentityUser> signInManagerService, 
                                 RoleManager<IdentityRole> roleManagerService)
        {
            _customerDataService = customerDataService;
            _userManagerService = userManagerService;
            _signInManagerService = signInManagerService;
            _roleManagerService = roleManagerService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(AccountRegisterViewModel vm)
        {
            if (ModelState.IsValid)
            {
                // add a new user to the table
                IdentityUser user = new IdentityUser(vm.UserName);
                user.Email = vm.Email;
                IdentityResult result = await _userManagerService.CreateAsync(user, vm.Password);

                if (result.Succeeded)
                {
                    Customer customer = new Customer
                    {
                        UserId = user.Id,
						FirstName = vm.FirstName,
						LastName = vm.LastName,
                        Email = vm.Email,
                        UserName = vm.UserName,
                        Phone = vm.Phone
                    };

                    _customerDataService.Create(customer);
					await _userManagerService.AddToRoleAsync(user, "Customer");

					//go home
					return RedirectToAction("SuccessfulRegistration", "Account");
                }
                else
                {
                    //show error
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View();
        }


        //get login form
        [HttpGet]
        public IActionResult Login(string returnUrl = "")
        {
            AccountLoginViewModel vm = new AccountLoginViewModel
            {
              
                ReturnUrl = returnUrl
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Login(AccountLoginViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManagerService.PasswordSignInAsync(vm.UserName, vm.Password, vm.RememberMe, false);

                if (result.Succeeded)
                {
                    if (String.IsNullOrEmpty(vm.ReturnUrl))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return Redirect(vm.ReturnUrl);
                    }
                }
                ModelState.AddModelError("", "Username or password invalid");
            }
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManagerService.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Denied()
        {
            return View();
        }

		[HttpGet]
		public IActionResult SuccessfulRegistration()
		{
			return View();
		}
	}
}