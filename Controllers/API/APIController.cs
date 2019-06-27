using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
//
using BiankaKorban_DiplomaProject.Models;
using BiankaKorban_DiplomaProject.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BiankaKorban_DiplomaProject.Controllers.API
{
    
    public class APIController : Controller
    {

        private  IDataService<Hamper> _hamperDataService;
        private  IDataService<Category> _categoryDataService;

        public APIController(IDataService<Hamper> hamperService,
                             IDataService<Category> categoryService)
        {
            _hamperDataService = hamperService;
            _categoryDataService = categoryService;
        }


        // GET: api/values
        [Route("api/Hampers")]
        [HttpGet]
        public IActionResult Get()
        {
        //    IEnumerable<Hamper> listOfHampers = _hamperDataService.GetAll();
            
        //    return Ok(listOfHampers.ToList());


            IEnumerable<Hamper> listOfHampers = _hamperDataService.GetAll();
            IEnumerable<Category> listOfCategories = _categoryDataService.GetAll();

            var result = listOfHampers.Join(
                listOfCategories,
                h => h.CategoryId,
                c => c.CategoryId,
                (h, c) => new
                {
                 HamperId = h.HamperId,
                 Name = h.Name,
                 Price = h.Price,
                 Image = h.Image,
                 Category = c.Name,
                 Description = h.Description
                 
                });


            return Ok(result);
        }
        
        [Route("api/Hampers/{id}")]
        //GET api/<controller>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {

            IEnumerable<Hamper> listOfHampers = _hamperDataService.GetAll();
            IEnumerable<Category> listOfCategories = _categoryDataService.GetAll();

            var result = listOfHampers.Join(
                listOfCategories,
                h => h.CategoryId,
                c => c.CategoryId,
                (h, c) => new
                {
                    HamperId = h.HamperId,
                    Name = h.Name,
                    Price = h.Price,
                    Image = h.Image,
                    Category = c.Name,
                    CategoryId = h.CategoryId,
                    Description = h.Description

                }).Where(h => h.CategoryId == id);

            return Ok(result);
        }
    }
}
