using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
//
using BiankaKorban_DiplomaProject.Services;
using BiankaKorban_DiplomaProject.Models;
using BiankaKorban_DiplomaProject.ViewModels;

namespace GrandeGift.Controllers
{
    public class FilterController : Controller
    {
		private IDataService<Hamper> _hamperDataService;
		private IDataService<Category> _categoryDataService;

		public FilterController(IDataService<Hamper> hamperService, IDataService<Category> categoryService)
		{
			_hamperDataService = hamperService;
			_categoryDataService = categoryService;
		}

		[HttpGet]
        public IActionResult Search(double min, double max, int id)
        {
			//call service
			//IEnumerable<Hamper> listOfHampers = _hamperDataService.Query(h => h.CategoryId == id).Where(h => h.Discontinue == false);

			IEnumerable<Hamper> listOfHampers = _hamperDataService.Query(h => h.CategoryId == id).Where(h => h.Discontinue == false);

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

			FilterSearchViewModel vm = new FilterSearchViewModel
			{
				Hampers = listOfHampers,
				max = max,
				min = min,
				Categories = listOfCategories,
				CategoryId = id
				
			};

		

			//pass to the view
			return View(vm);
        }

		public IActionResult Search(FilterSearchViewModel vm)
		{
			return RedirectToAction("Search", new { min = vm.min, max = vm.max, id = vm.CategoryId });
		}

		
	}
}