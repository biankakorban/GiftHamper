using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
//
using BiankaKorban_DiplomaProject.Services;
using BiankaKorban_DiplomaProject.Models;
using BiankaKorban_DiplomaProject.ViewModels;


namespace BiankaKorban_DiplomaProject.Controllers
{
    public class HomeController : Controller
    {
		private IDataService<Hamper> _hamperDataService;

		public HomeController(IDataService<Hamper> hamperService)
		{
			_hamperDataService = hamperService;
		}


        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

   

        [HttpGet]
        public IActionResult TermsOfUse()
        {
            return View();
        }

        [HttpGet]
        public IActionResult PrivacyPolicy()
        {
            return View();
        }

		[HttpGet]
		public IActionResult HamperList()
		{

			IEnumerable<Hamper> listOfHampers = _hamperDataService.GetAll();

			//vm
			HomeHamperListViewModel vm = new HomeHamperListViewModel
			{
				Hampers = listOfHampers
			};

			return View(vm);
		}
	}
}