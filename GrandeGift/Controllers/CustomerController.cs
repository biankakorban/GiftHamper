using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BiankaKorban_DiplomaProject.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult CustomerProfile()
        {
            return View();
        }
    }
}