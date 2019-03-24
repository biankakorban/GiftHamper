using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
//
using BiankaKorban_DiplomaProject.Services;
using BiankaKorban_DiplomaProject.Models;
using BiankaKorban_DiplomaProject.ViewModels;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BiankaKorban_DiplomaProject.Controllers
{
	public class HamperProductController : Controller
	{

		private IDataService<Hamper> _hamperDataService;
		private IDataService<Category> _categoryDataService;
		private IDataService<Product> _productDataService;
		private IDataService<HamperProduct> _hamperProductService;
		private HamperManager _hamperManager;
		

		public HamperProductController(IDataService<Hamper> hamperService,
									IDataService<Category> categoryService,
									IDataService<Product> productService,
									IDataService<HamperProduct> hamperProductService
								)
		{
			_hamperDataService = hamperService;
			_categoryDataService = categoryService;
			_productDataService = productService;
			_hamperProductService = hamperProductService;
			_hamperManager = new HamperManager();
		}

		[Route("HamperProduct/Create/{id}")]
		public IActionResult Create(int id)
		{

			//call service
			IEnumerable<Product> listOfProducts = _productDataService.GetAll();
			
			IEnumerable<Category> listOfCategories = _categoryDataService.GetAll();
			Hamper hamper = _hamperDataService.GetSingle(h => h.HamperId == id);
			Category category = _categoryDataService.GetSingle(c => c.CategoryId == hamper.CategoryId);
			//vm
			HamperProductCreateViewModel vm = new HamperProductCreateViewModel
			{
				HamperId = id,
				Hamper = hamper,
                Price = hamper.Price,
                Name = hamper.Name,
                
                CategoryList = listOfCategories,
				CategoryName = category.Name,
				ProductList = listOfProducts
			};


			//pass to the view
			return View(vm);


		}


		[Route("HamperProduct/Attach/{hamperId}/{productId}")]
		public IActionResult Attach(int hamperId, int productId)
		{

			_hamperManager.addProduct(hamperId, productId);

			return RedirectToAction("Create", new { hamperId});
		}

    }
}