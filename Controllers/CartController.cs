using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
//
using BiankaKorban_DiplomaProject.Services;
using BiankaKorban_DiplomaProject.Models;
using BiankaKorban_DiplomaProject.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;


using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;


namespace BiankaKorban_DiplomaProject.Controllers
{
	public class CartController : Controller
    {

        //A Helper to identify Hosting side path info 
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IDataService<Hamper> _hamperDataService;
        private IDataService<Order> _orderDataService;
        private IDataService<OrderLine> _orderLineDataService;
        private UserManager<IdentityUser> _userManagerService;
        private IDataService<Customer> _customerDataService;
        private IDataService<Address> _addressDataService;
        private CartManager _cartManager;
		

		public CartController(IHostingEnvironment hostingEnvironment,
                                 IDataService<Hamper> hamperService,
                                 IDataService<Order> orderService,
                                 IDataService<OrderLine> orderLineService,
                                 UserManager<IdentityUser> userManagerService,
                                  IDataService<Address> addressService,
                                  IDataService<Customer> customerService)
        {
            _hostingEnvironment = hostingEnvironment;
            _hamperDataService = hamperService;
            _orderDataService = orderService;
            _orderLineDataService = orderLineService;
            _userManagerService = userManagerService;
            _addressDataService = addressService;
            _customerDataService = customerService;
            _cartManager = new CartManager();

			
		}

		

		[HttpGet]
        public async Task<IActionResult> Checkout()
        {
            
            var userId = await _userManagerService.GetUserAsync(User);
            Customer customer = _customerDataService.GetSingle(us => us.UserId == userId.Id);


            List<Address> addressList = _addressDataService.Query(a => a.CustomerId == customer.CustomerId).ToList();
            addressList.Insert(0, new Address { AddressId = 0, Line1 = "Select address" });

            List<OrderLine> cart = SessionHelper.GetObjectFromJson<List<OrderLine>>(HttpContext.Session, "cart");

            //vm
            CartCheckoutViewModel vm = new CartCheckoutViewModel
            {
                Customer = customer,
                MyAddresses = addressList,
                OrderLineItems = cart.ToList()
            };

            return View(vm);
        }


        [HttpPost]
        public async Task<IActionResult> Checkout(CartCheckoutViewModel vm)
        {
            if (ModelState.IsValid && vm.AddressId != 0)
            {
                List<OrderLine> cart = SessionHelper.GetObjectFromJson<List<OrderLine>>(HttpContext.Session, "cart");

                var userId = await _userManagerService.GetUserAsync(User);
                Customer customer = _customerDataService.GetSingle(us => us.UserId == userId.Id);

                //List<Address> addressList = _addressDataService.Query(a => a.CustomerId == customer.CustomerId).ToList();
                //addressList.Insert(0, new Address { AddressId = 0, Line1 = "Select address" });

                //create a new order


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
                ////go back to Home/ Index
                return RedirectToAction("Index", "Customer");


            }
            //if invalid
            return View(vm);
        }



		//[Route("Cart/Attach/{orderId}/{addressId}")]
		//public IActionResult Attach(int orderId, int addressId)
		//{

		//    _orderDataService.Update(orderId, addressId);

		//    return RedirectToAction("Create", new { hamperId });
		//}


		[Route("Remove/{id}")]
		public IActionResult Remove(int id)
		{
			
			////We are checking if any product is previously added to the cart or not
		
			//	//Retriving the cart items from the session 
			//	var cart = SessionHelper.GetObjectFromJson<List<OrderLine>>(HttpContext.Session, "cart");
			//	//passing the cart to the ViewBag [ViewBag is a bag holding the data to share with view]
			//	ViewBag.cart = cart;
			//	//List<OrderLine> cart = SessionHelper.GetObjectFromJson<List<OrderLine>>(HttpContext.Session, "cart");
			//	cart.Remove(cart._hamperDataService.GetSingle(h => h.HamperId == id));

			//SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);

			//return RedirectToAction("Index", "Hamper");



			List<OrderLine> cart = SessionHelper.GetObjectFromJson<List<OrderLine>>(HttpContext.Session, "cart");
			int index = IsExist(id);
			cart.RemoveAt(index);
			SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
			return RedirectToAction("Index");
		}



        // GET: /<controller>/
        public IActionResult Index()
        {

            //Retriving the cart items from the session 
            var cart = SessionHelper.GetObjectFromJson<List<OrderLine>>(HttpContext.Session, "cart");
            //passing the cart to the ViewBag [ViewBag is a bag holding the data to share with view]
            ViewBag.cart = cart;
            if (cart != null)
            {
                //You can calcualte the total of your selected items in cart 
                ViewBag.totalQuantity = cart.Count();
                ViewBag.total = cart.Sum(item => item.Hamper.Price * item.Quantity);
            }
            return View();
        }


        [Route("Buy/{id}")]
        public IActionResult Buy(int id)
        {
            //We are checking if any product is previously added to the cart or not
            if (SessionHelper.GetObjectFromJson<List<OrderLine>>(HttpContext.Session, "cart") == null)
            {
                //If there is not item exist in the session 
                List<OrderLine> cart = new List<OrderLine>();
                cart.Add(new OrderLine { Hamper = _hamperDataService.GetSingle(h => h.HamperId == id), Quantity = 1 });
                //You saving your List of OrderItems inside the Sesson with a key called "cart" as a json data
                //Because json is basically string 
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            else
            {
                //If already there are OrderItems exist in the Session
                //We first retriving them and checking
                List<OrderLine> cart = SessionHelper.GetObjectFromJson<List<OrderLine>>(HttpContext.Session, "cart");
                //We have a method name IsExist which taking an argument is the product id
                //We checking if any product with that particular id is already added to the cart or not
                int index = IsExist(id);
                if (index != -1)
                {
                    //If already exist 
                    //Just increasing the quantity
                    cart[index].Quantity++;
                }
                else
                {
                    //If not exist
                    //I am adding the new orderItem to the existing list
                    //Hamper hamper = _hamperDataService.GetSingle(h => h.HamperId == id);
                    cart.Add(new OrderLine { Hamper = _hamperDataService.GetSingle(h => h.HamperId == id), Quantity = 1 });
                    //cart.Add(new OrderLine { HamperId = hamper.HamperId, Quantity = 1 });
                }
                //I am adding the new orderItem to the existing list
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
    }

	

}