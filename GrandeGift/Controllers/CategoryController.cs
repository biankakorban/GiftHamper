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
    public class CategoryController : Controller
    {



        private IDataService<Category> _categoryDataService;

        public CategoryController(IDataService<Category> categoryService)
        {
            _categoryDataService = categoryService;
        }

   
        [HttpGet]
        public IActionResult Create()
        {
            //call service
            IEnumerable<Category> listOfCategories = _categoryDataService.GetAll();

            //vm
            CategoryCreateViewModel vm = new CategoryCreateViewModel
            {
                Categories = listOfCategories,
                Total = listOfCategories.Count()
            };
            return View(vm);
        }


        
        [HttpPost]
        public IActionResult Create(CategoryCreateViewModel vm)
        {
            //check if the data is valid
            if (ModelState.IsValid)
            {
                //map vm to model
                Category category = new Category
                {
                    Name = vm.Name
                };
                //call the service
                _categoryDataService.Create(category);
                //go back to Home/Index
                return RedirectToAction("Details", "Category");
            }
            //if invalid
            return View(vm);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            //call services
            Category category = _categoryDataService.GetSingle(c => c.CategoryId == id);
            IEnumerable<Category> listOfCategories = _categoryDataService.GetAll();

            //map
            CategoryEditViewModel vm = new CategoryEditViewModel
            {
                CategoryId = category.CategoryId,
                Name = category.Name,
                Categories = listOfCategories,
                Total = listOfCategories.Count()
            };


            return View(vm);
        }


        [HttpPost]
        public IActionResult Edit(CategoryEditViewModel vm)
        {
            if (ModelState.IsValid)
            {
                Category category = new Category
                {
                    CategoryId = vm.CategoryId,
                    Name = vm.Name
                };

                //call service
                _categoryDataService.Update(category);

                //go to admin page with list of categories
                return RedirectToAction("Details", "Category");
            }

            //pass to the view
            return View(vm);
        }


        [HttpGet]
        public IActionResult Delete(int id)
        {
            _categoryDataService.Delete(new Category { CategoryId = id });
            return RedirectToAction("Details", "Category");
        }



        [HttpGet]
        public IActionResult Details()
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