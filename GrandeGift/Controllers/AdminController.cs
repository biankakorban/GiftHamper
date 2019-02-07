using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
//
using BiankaKorban_DiplomaProject.Models;
using BiankaKorban_DiplomaProject.Services;
using BiankaKorban_DiplomaProject.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace BiankaKorban_DiplomaProject.Controllers
{
    public class AdminController : Controller
    {

		private IDataService<Category> _categoryDataService;

		public AdminController(IDataService<Category> categoryService)
		{
			_categoryDataService = categoryService; 
		}


    
		[HttpGet]
        public IActionResult AdminPage()
        {
            //call service
            IEnumerable<Category> listOfCategories = _categoryDataService.GetAll();

            //vm
            CategoryDetailsViewModel vm = new CategoryDetailsViewModel
            {
                Categories = listOfCategories,
                Total = listOfCategories.Count()
            };


            //pass to the view
            return View(vm);
        }



	}
}