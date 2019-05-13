using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
//
using BiankaKorban_DiplomaProject.Models;
using BiankaKorban_DiplomaProject.Services;


namespace BiankaKorban_DiplomaProject.Controllers.API
{
    [Route("api/categories")]
    [ApiController]
    public class APICategoryController : ControllerBase
    {

        private IDataService<Hamper> _hamperDataService;
        private IDataService<Category> _categoryDataService;

        public APICategoryController(IDataService<Hamper> hamperService,
                             IDataService<Category> categoryService)
        {
            _hamperDataService = hamperService;
            _categoryDataService = categoryService;
        }



        [HttpGet]
        public IActionResult Get()
        {
            IEnumerable<Category> listOfCategories = _categoryDataService.GetAll();

            return Ok(listOfCategories.ToList());
        }
    }
}