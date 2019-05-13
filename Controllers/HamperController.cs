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
        //
        private readonly IHostingEnvironment _hostingEnvironmentService;



        public HamperController(IDataService<Hamper> hamperService,
                                IDataService<Category> categoryService,
                                IHostingEnvironment hostingEnvironmentService)
        {
            _hamperDataService = hamperService;
            _categoryDataService = categoryService;
            _hostingEnvironmentService = hostingEnvironmentService;

        }

        [HttpGet]
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
        public IActionResult Create()
        {
      

            //call service - get all categories and assing to list
            List<Category> categoryList = _categoryDataService.GetAll().ToList();
            //insert categories to list, start from index - select a category
            categoryList.Insert(0, new Category { CategoryId = 0, Name = "Select a category" });
            //display category list
            ViewBag.CategoryList = categoryList;

     
			return View();
        }

   
	
		
		[HttpPost]
        public async Task<IActionResult> Create(HamperCreateViewModel vm, IFormFile image)
        {
			Hamper hamper = new Hamper();

            //checking if the registration form sending a file or not
            if (image != null)
            {
                //Create a path including the filename where we want to save the file 
                var fileName = Path.Combine(_hostingEnvironmentService.WebRootPath, "imagesCopy", Path.GetFileName(image.FileName));
                //copy the file from temp memory to a permanent memory
                var fileStream = new FileStream(fileName, FileMode.Create);
                await image.CopyToAsync(fileStream);
                //Whenever you use any System.IO interface or classes you makesure you close the process;
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
				hamper.Description = vm.Description;

                _hamperDataService.Create(hamper);
                //go to list of products to attach products to hamper
                return RedirectToAction("Index", "Hamper");
            }
            return View(vm);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            Hamper hamper = _hamperDataService.GetSingle(h => h.HamperId == id);
            Category category = _categoryDataService.GetSingle(c => c.CategoryId == hamper.CategoryId);


            HamperDetailsViewModel vm = new HamperDetailsViewModel
            {
                HamperId = hamper.HamperId,
                Name = hamper.Name,
                Price = hamper.Price,
                Description = hamper.Description,
                CategoryId = hamper.CategoryId,
                CategoryName = category.Name,
                Image = hamper.Image

            };
            return View(vm);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
           
            Hamper hamper = _hamperDataService.GetSingle(h => h.HamperId == id);
            Category category = _categoryDataService.GetSingle(c => c.CategoryId == hamper.CategoryId);

            List<Category> categoryList = _categoryDataService.GetAll().ToList();
            //insert categories to list, start from index - select a category
            categoryList.Insert(0, new Category { CategoryId = 0, Name = "Select a category" });
            //display category list
            ViewBag.CategoryList = categoryList;

            HamperEditViewModel vm = new HamperEditViewModel
            {
                HamperId = hamper.HamperId,
                Name = hamper.Name,
                Price = hamper.Price,
                Description = hamper.Description,
                CategoryId = hamper.CategoryId,
                Image = hamper.Image,
                Discontinued = hamper.Discontinued

            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, HamperEditViewModel vm, IFormFile image)
        {
            if (ModelState.IsValid)
            {

                Hamper hamper = _hamperDataService.GetSingle(h => h.HamperId == vm.HamperId);
                Category category = _categoryDataService.GetSingle(c => c.CategoryId == vm.CategoryId);

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
                hamper.Description = vm.Description;
                hamper.CategoryId = vm.CategoryId;

                hamper.Discontinued = vm.Discontinued;
                
                


                //call service
                _hamperDataService.Update(hamper);
                // profile.DOB = vm.DOB;
                vm.Image = hamper.Image; //I just updated view model from the DbModel 

          

                //go home/index
                return RedirectToAction("Index", "Hamper");
            }
            
            
            return View(vm);
        }

        [HttpGet]
        public IActionResult Search()
        {
            

            List<Category> categoryList = _categoryDataService.GetAll().ToList();
            //insert categories to list, start from index - select a category
            categoryList.Insert(0, new Category { CategoryId = 0, Name = "Select a category" });
            //display category list
            ViewBag.CategoryList = categoryList;

            


            return View();
        }


        [HttpPost]
        public IActionResult Search(HamperSearchViewModel vm)
        {
            Category category = _categoryDataService.GetSingle(c => c.CategoryId == vm.CategoryId);

            IEnumerable<Hamper> hamperList = _hamperDataService.Query(h => h.CategoryId == category.CategoryId);
            //Hamper hamper = _hamperDataService.GetSingle(h => h.CategoryId == category.CategoryId);

            Hamper hamper = new Hamper();

            ////getting selected value
            int selectValue = vm.CategoryId;

            //setting data back to viewbag after posting form
            //List<Category> categoryList = new List<Category>();

            //categoryList = _categoryDataService.GetAll().ToList();
            //categoryList.Insert(0, new Category { CategoryId = 0, Name = "Select a category" });

            //ViewBag.CategoryList = categoryList;

            foreach (var ham in hamperList)
            {
                hamper.CategoryId = vm.CategoryId;
            }

           

            if (hamper != null)
            {
                //call service
                //IEnumerable<Hamper> listOfHampers = _hamperDataService.Query(h => h.CategoryId == hamper.CategoryId);
                IEnumerable<Hamper> listOfHampers = _hamperDataService.GetAll();
                //vm
                HamperSearchViewModel viewModel = new HamperSearchViewModel
                {
                    Hampers = listOfHampers
                };


          

                return RedirectToAction("FoundHampers", "Hamper");
            }

            ModelState.AddModelError("", "can not find this hamper.");
            return View(vm);
        }


        [HttpGet]
        public IActionResult FoundHampers()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GiftForHer()
        {

            IEnumerable<Hamper> listOfHampersCat = _hamperDataService
                .Query(h => h.CategoryId == 2003)
                .Where(h => h.Discontinued == false);

            //vm
            HamperSearchViewModel vm = new HamperSearchViewModel
            {
                Hampers = listOfHampersCat
            };


            //pass to the view
            return View(vm);
        }

        [HttpGet]
        public IActionResult HampersForEveryOccassion()
        {

            IEnumerable<Hamper> listOfHampersCat = _hamperDataService
                .Query(h => h.CategoryId == 2004)
                .Where(h => h.Discontinued == false);

            //vm
            HamperSearchViewModel vm = new HamperSearchViewModel
            {
                Hampers = listOfHampersCat
            };


            //pass to the view
            return View(vm);
        }
        [HttpGet]
        public IActionResult HampersForBaby()
        {

            IEnumerable<Hamper> listOfHampersCat = _hamperDataService
                .Query(h => h.CategoryId == 3)
                .Where(h => h.Discontinued == false);

            //vm
            HamperSearchViewModel vm = new HamperSearchViewModel
            {
                Hampers = listOfHampersCat
            };


            //pass to the view
            return View(vm);
        }
        [HttpGet]
        public IActionResult GiftForHim()
        {

            IEnumerable<Hamper> listOfHampersCat = _hamperDataService
                .Query(h => h.CategoryId == 2)
                .Where(h => h.Discontinued == false);

            //vm
            HamperSearchViewModel vm = new HamperSearchViewModel
            {
                Hampers = listOfHampersCat
            };


            //pass to the view
            return View(vm);
        }

       


    }
}