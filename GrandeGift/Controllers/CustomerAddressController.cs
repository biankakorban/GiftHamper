using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
//
using BiankaKorban_DiplomaProject.Models;
using BiankaKorban_DiplomaProject.Services;
using BiankaKorban_DiplomaProject.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace BiankaKorban_DiplomaProject.Controllers
{
	public class CustomerAddressController : Controller
	{

		private IDataService<Customer> _customerDataService;
		private IDataService<Address> _addressDataService;
		private UserManager<IdentityUser> _userManagerService;

		public CustomerAddressController(IDataService<Customer> customerService,
										IDataService<Address> addressService,
										UserManager<IdentityUser> userManagerService)
		{
			_customerDataService = customerService;
			_addressDataService = addressService;
			_userManagerService = userManagerService;
		}

		[HttpGet]
		public IActionResult Create()
		{
			var currentUserId = _userManagerService.GetUserId(HttpContext.User);
			Customer customer = _customerDataService.GetSingle(us => us.UserId == currentUserId);


			CustomerAddressCreateViewModel vm = new CustomerAddressCreateViewModel
			{
				CustomerId = customer.CustomerId
			};
			return View(vm);
		}

		[HttpPost]
		public IActionResult Create(CustomerAddressCreateViewModel vm)
		{
			if (ModelState.IsValid)
			{
				//map
				Address address = new Address
				{
					CustomerId = vm.CustomerId,
					Line1 = vm.Line1,
					Line2 = vm.Line2,
					City = vm.City,
					State = vm.State,
					Suburb = vm.Suburb,
					Country = vm.Country,
					PostalCode = vm.PostalCode

				};
				//call service
				_addressDataService.Create(address);

				//go home/index
				return RedirectToAction("Details", "CustomerProfile");


			}
			return View(vm);
		}

		[HttpGet]
		public IActionResult Details(int id)
		{

			Address address = _addressDataService.GetSingle(a => a.AddressId == id);

			CustomerAddressDetailsViewModel vm = new CustomerAddressDetailsViewModel
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
			return View(vm);

		}


		[HttpGet]
		public IActionResult Edit(int id)
		{
			Address address = _addressDataService.GetSingle(a => a.AddressId == id);

			CustomerAddressEditViewModel vm = new CustomerAddressEditViewModel
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


			return View(vm);
		}

		[HttpPost]
		public IActionResult Edit(CustomerAddressEditViewModel vm)
		{
			if (ModelState.IsValid)
			{

				Address address = _addressDataService.GetSingle(a => a.AddressId == vm.AddressId);


				address.Line1 = vm.Line1;
				address.Line2 = vm.Line2;
				address.State = vm.State;
				address.City = vm.City;
				address.Country = vm.Country;
				address.Suburb = vm.Suburb;
				address.PostalCode = vm.PostalCode;


				//call service
				_addressDataService.Update(address);

				//go home/index
				return RedirectToAction("Details", "CustomerProfile");
			}
			return View(vm);
		}

		[HttpGet]
		public IActionResult Delete(int id)
		{
			_addressDataService.Delete(new Address { AddressId = id });
			return RedirectToAction("Details", "CustomerProfile");
		}

		//[HttpGet]
		//public IActionResult Delete(int id)
		//{
		//	Address address = _addressDataService.GetSingle(a => a.AddressId == id);

		//	CustomerAddressEditViewModel vm = new CustomerAddressEditViewModel
		//	{
		//		AddressId = address.AddressId,
		//		Line1 = address.Line1,
		//		Line2 = address.Line2,
		//		City = address.City,
		//		State = address.State,
		//		Suburb = address.Suburb,
		//		PostalCode = address.PostalCode,
		//		Country = address.Country
		//	};


		//	return View(vm);
		//}

		//[HttpPost]
		//public IActionResult Delete(CustomerAddressDeleteViewModel vm)
		//{
		//	if (ModelState.IsValid)
		//	{

		//		Address address = _addressDataService.GetSingle(a => a.AddressId == vm.AddressId);


		//		address.Line1 = vm.Line1;
		//		address.Line2 = vm.Line2;
		//		address.State = vm.State;
		//		address.City = vm.City;
		//		address.Country = vm.Country;
		//		address.Suburb = vm.Suburb;
		//		address.PostalCode = vm.PostalCode;


		//		//call service
		//		_addressDataService.Delete(address);

		//		//go home/index
		//		return RedirectToAction("Details", "CustomerProfile");
		//	}
		//	return View(vm);

		//}
	}
}