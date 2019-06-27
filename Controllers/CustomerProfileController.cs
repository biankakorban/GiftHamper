using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
//...
using BiankaKorban_DiplomaProject.Services;
using BiankaKorban_DiplomaProject.ViewModels;
using BiankaKorban_DiplomaProject.Models;



namespace BiankaKorban_DiplomaProject.Controllers
{
    public class CustomerProfileController : Controller
    {

        private IDataService<Customer> _customerDataService;
        private IDataService<Address> _addressDataService;
        private UserManager<IdentityUser> _userManagerService;

        public CustomerProfileController(IDataService<Customer> customerService,
                                        IDataService<Address> addressService,
                                        UserManager<IdentityUser> userManagerService)
        {
            _customerDataService = customerService;
            _addressDataService = addressService;
            _userManagerService = userManagerService;
        }




        [HttpGet]
        public IActionResult Details()
        {


            var currentUserId = _userManagerService.GetUserId(HttpContext.User);
            Customer customer = _customerDataService.GetSingle(us => us.UserId == currentUserId);
            //var currentCustomerId = _customerDataService.GetSingle(us => us.UserId == currentUserId);

			IEnumerable<Address> addressList = _addressDataService.Query(a => a.CustomerId == customer.CustomerId);
			List<CustomerAddressDetailsViewModel> addressListVm = new List<CustomerAddressDetailsViewModel>();


			foreach (var address in addressList)
			{
				CustomerAddressDetailsViewModel addressViewModel = new CustomerAddressDetailsViewModel
				{
				
					AddressId = address.AddressId,
					Line1 = address.Line1,
					Line2 = address.Line2,
					City = address.City,
					State = address.State,
					Suburb = address.Suburb,
					PostalCode = address.PostalCode,
					Country = address.Country
				};
				addressListVm.Add(addressViewModel);

			}

			//vm
			CustomerProfileDetailsViewModel vm = new CustomerProfileDetailsViewModel
            {
                FirstName = customer.FirstName,
                CustomerId = customer.CustomerId,
                LastName = customer.LastName,
                Email = customer.Email,
                Phone = customer.Phone,
                UserName = customer.UserName,
                MyAddresses = addressListVm,
            };

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {

			var currentUserId = _userManagerService.GetUserId(HttpContext.User);
			Customer customer = _customerDataService.GetSingle(us => us.UserId == currentUserId);
			//var currentCustomerId = _customerDataService.GetSingle(us => us.UserId == currentUserId);
			//var currentUserId = _userManagerService.GetUserId(HttpContext.User);
			//Customer customer = _customerDataService.GetSingle(us => us.CustomerId == id);
			var userId = customer.UserId;
			IdentityUser user = await _userManagerService.FindByIdAsync(userId);

			//var currentCustomerId = _customerDataService.GetSingle(us => us.CustomerId == id);

			CustomerProfileEditViewModel vm = new CustomerProfileEditViewModel
            {
                CustomerId = customer.CustomerId,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Phone = customer.Phone,
                UserName = customer.UserName,
				//User = user,
				Email = customer.Email,
				UserId = user.Id
            };
			
            return View(vm);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(CustomerProfileEditViewModel vm)
        {
            if (ModelState.IsValid)
            {
				
				var currentUser = await _userManagerService.GetUserAsync(User);
				currentUser.Email = vm.Email;
				currentUser.UserName = vm.UserName;
				IdentityResult result = await _userManagerService.UpdateAsync(currentUser);


				Customer customer = new Customer
                    {
                        UserId = vm.UserId,
                        CustomerId = vm.CustomerId,
                        FirstName = vm.FirstName,
                        LastName = vm.LastName,
                        UserName = vm.UserName,
                        Phone = vm.Phone,
						Email = vm.Email
				};
                   
                    _customerDataService.Update(customer);


                    //go to customer profile
                    return RedirectToAction("Details", "CustomerProfile");
             
            }
            return View();
        }


    
    }
}
