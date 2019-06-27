using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BiankaKorban_DiplomaProject.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Customer()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Admin()
        {
            return View();
        }
    }
}