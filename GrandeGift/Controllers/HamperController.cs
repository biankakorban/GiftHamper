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
using Microsoft.EntityFrameworkCore;

namespace BiankaKorban_DiplomaProject.Controllers
{
    public class HamperController : Controller
    {



      


        private IDataService<Hamper> _hamperDataService;
        private IDataService<Category> _categoryDataService;
        private IDataService<Product> _productDataService;
        private IDataService<HamperProduct> _hamperProductDataService;
        private MyDbContext _context;
        //
        private readonly IHostingEnvironment _hostingEnvironmentService;
        private HamperManager _hamperManager;
        private DbSet<Hamper> _dbHamper;
        private DbSet<Product> _dbProduct;
        private DbSet<HamperProduct> _dbHamperProduct;



        public HamperController(IDataService<Hamper> hamperService,
                                IDataService<Category> categoryService,
                                IDataService<Product> productService,
                                IHostingEnvironment hostingEnvironmentService,
                                IDataService<HamperProduct> hamperProductService) 
        {
            _hamperDataService = hamperService;
            _categoryDataService = categoryService;
            _productDataService = productService;
            _hostingEnvironmentService = hostingEnvironmentService;
            _hamperManager = new HamperManager();
            _hamperProductDataService = hamperProductService;
            _context = new MyDbContext();
            _dbHamper = _context.Set<Hamper>();
            _dbProduct = _context.Set<Product>();
            _dbHamperProduct = _context.Set<HamperProduct>();

        }


		public IActionResult Index()
		{

			//call service
			IEnumerable<Hamper> listOfHampers = _hamperDataService.GetAll();

			//vm
			HamperIndexViewModel vm = new HamperIndexViewModel
			{
				Hampers = listOfHampers
			};


			//pass to the view
			return View(vm);
		}



        [HttpGet]
        public IActionResult Details(int id)
        {
            Hamper hamper = _hamperDataService.GetSingle(h => h.HamperId == id);
            Category category = _categoryDataService.GetSingle(c => c.CategoryId == hamper.CategoryId);
            IEnumerable<Hamper> hamperList = _hamperDataService.Query(h => h.HamperId == hamper.HamperId);
            HamperProduct hamperProduct = _hamperProductDataService.GetSingle(h => h.HamperId == hamper.HamperId);





            var productList = _hamperDataService.Query(a => a.HamperId == hamperProduct.HamperId);
            
            
          

            List<ProductDetailsViewModel> productListVm = new List<ProductDetailsViewModel>();


            foreach (var p in productList)
            {
                ProductDetailsViewModel productListViewModel = new ProductDetailsViewModel
                {
                    Name = p.Name

                };
                productListVm.Add(productListViewModel);

            }

            //vm
            HamperDetailsViewModel vm = new HamperDetailsViewModel
            {
                HamperId = hamper.HamperId,
                Name = hamper.Name,
                Price = hamper.Price,
                CategoryName = category.Name,
                ProductList = productListVm
                
            };

            return View(vm);
            
        }



        [HttpGet]
        public IActionResult Create()
        {
            //call service
            IEnumerable<Product> listOfProducts = _productDataService.GetAll();

            //vm
            HamperCreateViewModel vm = new HamperCreateViewModel
            {
                Products = listOfProducts.ToList(),
                Total = listOfProducts.Count()
            };

           

            //call service - get all categories and assing to list
            List<Category> categoryList = _categoryDataService.GetAll().ToList();
            //insert categories to list, start from index - select a category
            categoryList.Insert(0, new Category { CategoryId = 0, Name = "Select a category" });
            //display category list
            ViewBag.CategoryList = categoryList;

     
			return View(vm);
        }

   
	
		
		[HttpPost]
        public async Task<IActionResult> Create(HamperCreateViewModel vm, IFormFile image)
        {
			Hamper hamper = new Hamper();

			//check if the form is send
			if (image != null)
            {
                //create a path which include the filename we want to save the file
                var fileName = Path.Combine(_hostingEnvironmentService.WebRootPath, "images", Path.GetFileName(image.FileName));
                //copy the file from temp memory to a permanent memory
                var fileStream = new FileStream(fileName, FileMode.Create);
                await image.CopyToAsync(fileStream);
                //close System.IO inerface/class process
                fileStream.Close();
                hamper.Image = Path.GetFileName(image.FileName);
            }

            ////validation
            if (vm.CategoryId == 0)
            {
                ModelState.AddModelError("", "Select category");

            }

            ////getting selected value
            int selectValue = vm.CategoryId;

            //setting data back to viewbag after posting form
            List<Category> categoryList = new List<Category>();

            categoryList = _categoryDataService.GetAll().ToList();
            categoryList.Insert(0, new Category { CategoryId = 0, Name = "Select a category" });

            ViewBag.CategoryList = categoryList;


			if (ModelState.IsValid)
            {
                //add a new hamper to the database
                Hamper ham = new Hamper
                {
                    Name = vm.Name,
                    Price = vm.Price,
                    CategoryId = vm.CategoryId,
                    Image = vm.Image
                };

            


                _hamperDataService.Create(ham);
                //go to list of products to attach products to hamper
                
                return RedirectToAction("Create", "HamperProduct", new { id = ham.HamperId});
            }
            return View(vm);
        }




    }
}