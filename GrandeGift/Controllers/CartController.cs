using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
//
using Microsoft.AspNetCore.Http;
using BiankaKorban_DiplomaProject.Services;
using GrandeGift.Services;
using BiankaKorban_DiplomaProject.Models;
using GrandeGift.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace GrandeGift.Controllers
{
    public class CartController : Controller
    {

		private IDataService<Hamper> _hamperDataService;
		private IDataService<Order> _orderDataService;
		private IDataService<OrderLine> _orderLineDataService;
		private UserManager<IdentityUser> _userManagerService;
		private IDataService<Customer> _customerDataService;
		private IDataService<Address> _addressDataService;

		public CartController(IDataService<Hamper> hamperService,
							  IDataService<Order> orderService,
							  IDataService<OrderLine> orderLineService,
							  UserManager<IdentityUser> userManagerService,
							  IDataService<Customer> customerService,
							  IDataService<Address> addressService)
		{
			_hamperDataService = hamperService;
			_orderDataService = orderService;
			_orderLineDataService = orderLineService;
			_userManagerService = userManagerService;
			_customerDataService = customerService;
			_addressDataService = addressService;
		}

		[Route("Buy/{id}")]
		public IActionResult Buy(int id)
		{
			//check if any product is previously added to the cart or not
			if (SessionHelper.GetObjectFromJson<List<OrderLine>>(HttpContext.Session, "cart") == null)
			{
				//if the item does not exist in the session
				List<OrderLine> cart = new List<OrderLine>();
				//add hamper to OrderLineTbl
				cart.Add(new OrderLine { Hamper = _hamperDataService.GetSingle(h => h.HamperId == id), Quantity = 1 });
				//save the list of OrderLine items inside the session with a key called "cart" as a json data
				//because json is basically string
				SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
			}
			else
			{
				//if OrderLine items exist in the session
				//retrieve them from the session and check
				List<OrderLine> cart = SessionHelper.GetObjectFromJson<List<OrderLine>>(HttpContext.Session, "cart");
				//there is a method called isExist that takes as an argument the hamperId 
				//to check if this hamper was already added to the list
				//check if hamper with that particular id is already added to the cart or not
				int index = IsExist(id);
				if (index != -1)
				{
					//if already exist
					//increasy the quantity, do not add new hamper with the same id
					cart[index].Quantity++;
				}
				else
				{
					//if hamper with that id does not exist
					//add the new OrderLine item to the existing list
					cart.Add(new OrderLine { Hamper = _hamperDataService.GetSingle(h => h.HamperId == id), Quantity = 1 });
				}

				//add the new OrderLine item to the existing list
				SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
			}
			return RedirectToAction("Index");
		}


		private int IsExist(int id)
		{
			List<OrderLine> cart = SessionHelper.GetObjectFromJson<List<OrderLine>>(HttpContext.Session, "cart");
			for (int i = 0; i < cart.Count; i++)
			{
				if (cart[i].Hamper.HamperId.Equals(id))
				{
					return i;
				}
			}
			return -1;
		}

		[HttpGet]
        public IActionResult Index()
        {
			//retrieving the cart items from the session
			var cart = SessionHelper.GetObjectFromJson<List<OrderLine>>(HttpContext.Session, "cart");
			//passing the cart to the ViewBag (ViewBag is a bug that is holding data to share with the view)
			ViewBag.cart = cart;
			if (cart != null)
			{
				//calculate the total of selected items in cart
				ViewBag.totalQuantity = cart.Sum(item => item.Quantity);
				ViewBag.total = cart.Sum(item => item.Hamper.Price * item.Quantity);
			}
            return View();
        }

		[Route("Remove/{id}")]
		public IActionResult Remove(int id)
		{
			//retrieve item from session
			List<OrderLine> cart = SessionHelper.GetObjectFromJson<List<OrderLine>>(HttpContext.Session, "cart");
			//check if item is on the list of items
			int index = IsExist(id);
			//remove from sessiona cart by using the specified index of the list
			if (index != -1)
			{
				cart.RemoveAt(index);
			}
			//pass items back to the session
			SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
			//return to the index
			return RedirectToAction("Index");
		}

		[HttpGet]
		public async Task<IActionResult> Checkout()
		{
			//retrive user id
			var userId = await _userManagerService.GetUserAsync(User);
			//retrieve user 
			Customer customer = _customerDataService.GetSingle(user => user.UserId == userId.Id);

			//retrieve the list of customer addresses
			List<Address> listOfAddresses = _addressDataService.Query(a => a.CustomerId == customer.CustomerId).ToList();
			//add addresses to the list
			listOfAddresses.Insert(0, new Address { AddressId = 0, Line1 = "Select address" });

			//get cart object with list of items added to the cart from session
			List<OrderLine> cart = SessionHelper.GetObjectFromJson<List<OrderLine>>(HttpContext.Session, "cart");

			//vm
			CartCheckoutViewModel vm = new CartCheckoutViewModel
			{
				Customer = customer,
				MyAddresses = listOfAddresses,
				OrderLineItems = cart.ToList()
			};

			return View(vm);
		}

		[HttpPost]
		public async Task<IActionResult> Checkout(CartCheckoutViewModel vm)
		{
			//check if vm is valid
			if (ModelState.IsValid && vm.AddressId != 0)
			{
				//retrieve cart object from the session
				List<OrderLine> cart = SessionHelper.GetObjectFromJson<List<OrderLine>>(HttpContext.Session, "cart");

				//retrive user id
				var userId = await _userManagerService.GetUserAsync(User);
				//retrieve user 
				Customer customer = _customerDataService.GetSingle(user => user.UserId == userId.Id);

				//map vm to model
				
				Order order = new Order
				{
					CustomerId = customer.CustomerId,
					OrderDate = DateTime.Now,
					AddressId = vm.AddressId
				};

				//call the service
				_orderDataService.Create(order);

				foreach (var item in cart)
				{
					//add to joining table
					OrderLine orderLine = new OrderLine
					{
						OrderId = order.OrderId,
						HamperId = item.Hamper.HamperId,
						Quantity = item.Quantity
					};

					_orderLineDataService.Create(orderLine);
				}

			

				//go back to Index
				return RedirectToAction("Index", "Home");
			}
			//if invalid return view
			return View(vm);
		}

    }
}