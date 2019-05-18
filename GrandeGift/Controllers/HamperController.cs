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

		[HttpGet]
		public IActionResult Index()
		{
			//var vM = new HamperIndexViewModel();
			//vM.Hampers = await _context.TblHamper
			//	.Include(i => i.Products)
			//	.ThenInclude(i => i.product)
			//	 .AsNoTracking()
			//	 .ToListAsync();


			//call service
			IEnumerable<Hamper> listOfHampers = _hamperDataService.GetAll();

			//vm
			HamperIndexViewModel vm = new HamperIndexViewModel
			{
				Hampers = listOfHampers,
				
			};


			//pass to the view
			return View(vm);
		}



		[HttpGet]
		public async Task<IActionResult> Details(int id)
		{

			//var vm = new HamperDetailsViewModel();

			Hamper hamper = _hamperDataService.GetSingle(h => h.HamperId == id);
			Category category = _categoryDataService.GetSingle(c => c.CategoryId == hamper.CategoryId);
			IEnumerable<Product> productList = _productDataService.GetAll();
			IEnumerable<HamperProduct> hamperProductList = _hamperProductDataService.Query(h => h.HamperId == hamper.HamperId);
				//.Where(p => p.product == productList);
			List<ProductDetailsViewModel> productListVM = new List<ProductDetailsViewModel>();

			//vm.HamperList = await _context.TblHamper
			//	.Include(i => i.Products)
			//	.ThenInclude(i => i.product)
			//	.Where(h => h.HamperId == hamper.HamperId)
			//	 .AsNoTracking()
			//	 .ToListAsync();

			await _context.TblHamper
				.Include(i => i.Products)
				.ThenInclude(i => i.product)
				.Where(h => h.HamperId == hamper.HamperId)
				 .AsNoTracking()
				 .ToListAsync();



			//vm
			HamperDetailsViewModel vm = new HamperDetailsViewModel
			{
				HamperId = hamper.HamperId,
				Name = hamper.Name,
				Image = hamper.Image,
				Price = hamper.Price,
				CategoryName = category.Name,
				HamperList = await _context.TblHamper
				.Include(i => i.Products)
				.ThenInclude(i => i.product)
				.Where(h => h.HamperId == hamper.HamperId)
				 .AsNoTracking()
				 .ToListAsync()
			
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
                var fileName = Path.Combine(_hostingEnvironmentService.WebRootPath, "imagesCopy", 
					Path.GetFileName(image.FileName));
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

				hamper.Name = vm.Name;
				hamper.Price = vm.Price;
				hamper.CategoryId = vm.CategoryId;
                
				

                _hamperDataService.Create(hamper);
                //go to list of products to attach products to hamper
                
                return RedirectToAction("Create", "HamperProduct", new { id = hamper.HamperId});
            }
            return View(vm);
        }


		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			//call services
			Hamper hamper = _hamperDataService.GetSingle(h => h.HamperId == id);
			Category category = _categoryDataService.GetSingle(c => c.CategoryId == hamper.CategoryId);



			var categoryList = _categoryDataService.GetAll().ToList();
			//insert categories to list, start from index - select a category
			categoryList.Insert(0, new Category { CategoryId = 0, Name = "Select a category" });
			//display category list
			ViewBag.CategoryList = categoryList;

			IEnumerable<Product> productList = _productDataService.GetAll();


			//map
			//vm
			HamperEditViewModel vm = new HamperEditViewModel
			{
				HamperId = hamper.HamperId,
				Name = hamper.Name,
				Image = hamper.Image,
				Price = hamper.Price,
				CategoryId = hamper.CategoryId,
				Products = productList.ToList(),
				HamperList = await _context.TblHamper
				.Include(i => i.Products)
				.ThenInclude(i => i.product)
				.Where(h => h.HamperId == hamper.HamperId)
				 .AsNoTracking()
				 .ToListAsync(),
				Discontinue = hamper.Discontinue
			};
	

			return View(vm);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(int id, HamperEditViewModel vm,  IFormFile image)
		{
			if (ModelState.IsValid)
			{
				Hamper hamper = _hamperDataService.GetSingle(h => h.HamperId == vm.HamperId);
				Category category = _categoryDataService.GetSingle(c => c.CategoryId == vm.CategoryId);

				//validation
				if (vm.CategoryId == 0)
				{
					ModelState.AddModelError("", "Select category");
				}

				////getting selected value
				int selectValue = vm.CategoryId;

				//////setting data back to viewbag after posting form
				List<Category> categoryList = new List<Category>();

				categoryList = _categoryDataService.GetAll().ToList();
				categoryList.Insert(0, new Category { CategoryId = 0, Name = "Select a category" });

				ViewBag.CategoryList = categoryList;

				string prevImagePath = null;
				if (image != null)
				{
					//Checking if previously any avatar was uploaded or not
					if (hamper.Image != null)
						{
							//As there was an avatar before so you have to delete it first 
							prevImagePath = Path.Combine(_hostingEnvironmentService.WebRootPath, "imagesCopy", hamper.Image);
							System.IO.File.Delete(prevImagePath);
						}

						var fileName = Path.Combine(_hostingEnvironmentService.WebRootPath, "imagesCopy", Path.GetFileName(image.FileName));
						var fileStream = new FileStream(fileName, FileMode.Create);
						await image.CopyToAsync(fileStream);
						fileStream.Close();

						hamper.Image = Path.GetFileName(image.FileName);
				}				

					hamper.Name = vm.Name;
					hamper.Price = vm.Price;
					hamper.CategoryId = selectValue;
					hamper.Discontinue = vm.Discontinue;
				

					//call service 
					_hamperDataService.Update(hamper);
					// profile.DOB = vm.DOB;
					vm.Image = hamper.Image ; //I just updated view model from the DbModel 

				IEnumerable<Product> productList = _productDataService.GetAll();
				vm.Products = productList.ToList();

				vm.HamperList = await _context.TblHamper
						.Include(i => i.Products)
							.ThenInclude(i => i.product)
						.Where(h => h.HamperId == hamper.HamperId)
							.AsNoTracking()
							.ToListAsync();
				////go home/index
				return RedirectToAction("Index", "Hamper");
		}
				//pass to the view
				return View(vm);
	}

		[Route("Hamper/Attach/{hamperId}/{productId}")]
		public IActionResult Attach(int hamperId, int productId)
		{

			_hamperManager.addProduct(hamperId, productId);

			return RedirectToAction("Edit", "Hamper", new { id = hamperId });
		}


		//[Route("Hamper/Remove/{hamperId}/{productId}")]
		//public IActionResult Remove(int hamperId, int productId)
		//{
		//	_hamperManager.removeProduct(hamperId, productId);

		//	return RedirectToAction("Edit", "Hamper", new { id = hamperId });
		//}

		[Route("/Hamper/Remove/{hamperId}/{productId}")]
		public IActionResult Remove(int productId, int hamperId)
		{
			_hamperManager.RemoveProduct(hamperId, productId);

			return RedirectToAction("Edit", "Hamper", new { id = hamperId });

		}


	}
}