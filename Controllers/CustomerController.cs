using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
//
using BiankaKorban_DiplomaProject.Models;
using BiankaKorban_DiplomaProject.ViewModels;
using BiankaKorban_DiplomaProject.Services;
using Microsoft.AspNetCore.Hosting;

namespace BiankaKorban_DiplomaProject.Controllers
{
    public class CustomerController : Controller
    {

        private IDataService<Hamper> _hamperDataService;
        private IDataService<Category> _categoryDataService;
        //
        private readonly IHostingEnvironment _hostingEnvironmentService;



        public CustomerController(IDataService<Hamper> hamperService,
                                IDataService<Category> categoryService,
                                IHostingEnvironment hostingEnvironmentService)
        {
            _hamperDataService = hamperService;
            _categoryDataService = categoryService;
            _hostingEnvironmentService = hostingEnvironmentService;

        }

        [HttpGet]
        public IActionResult Index(double min, double max)
        { 

            //call service
            IEnumerable<Hamper> listOfHampers = _hamperDataService.GetAll()
            .Where(h => h.Discontinued == false);
            

            //vm
            CustomerIndexViewModel vm = new CustomerIndexViewModel
            {
                Hampers = listOfHampers,
                MaxPrice = max,
                MinPrice = min
            };


            if (min != 0 && max != 0)
            {
                vm.Hampers = _hamperDataService.GetAll()
                .Where(h => h.Price >= min && h.Price <= max && h.Discontinued == false);
            }

            //pass to the view
            return View(vm);
        }



        [HttpGet]
        public IActionResult Search(double min, double max, int id)
        {

            //call service
            IEnumerable<Hamper> listOfHampers = _hamperDataService.Query(h => h.CategoryId == id).Where(h => h.Discontinued == false);

            IEnumerable<Category> listOfCategories = _categoryDataService.GetAll();

            if (id > 0)
            {
                listOfHampers = listOfHampers.Where(h => h.CategoryId == id);
            }
            if (max > 0)
            {
                listOfHampers = listOfHampers.Where(h => h.Price <= max);
            }
            if (min > 0)
            {
                listOfHampers = listOfHampers.Where(h => h.Price >= min);
            }
            
            //vm
            SearchHamperViewModel vm = new SearchHamperViewModel
            {
                Hampers = listOfHampers,
                max = max,
                min = min,
                CategoryList = listOfCategories,
                CategoryId = id
            };


            

            //pass to the view
            return View(vm);
        }

        public IActionResult Search(SearchHamperViewModel vm)
        {
            return RedirectToAction("Search", new { min = vm.min, max = vm.max, id = vm.CategoryId });
        }

    }

    
}

    
