using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
//
using BiankaKorban_DiplomaProject.Models;
using BiankaKorban_DiplomaProject.ViewModels;
using BiankaKorban_DiplomaProject.Services;

namespace BiankaKorban_DiplomaProject.Controllers
{
    public class ProductController : Controller
    {

        private IDataService<Product> _productDataService;
        private IDataService<Category> _categoryDataService;
        private IDataService<Hamper> _hamperDataService;

        public ProductController(IDataService<Product> productService,
                                 IDataService<Category> categoryService,
                                 IDataService<Hamper> hamperService)
        {
            _productDataService = productService;
            _categoryDataService = categoryService;
            _hamperDataService = hamperService;
        }

        [HttpGet]
        public IActionResult Create()
        {
            //call service
            IEnumerable<Product> listOfProducts = _productDataService.GetAll();

            //vm
            ProductCreateViewModel vm = new ProductCreateViewModel
            {
                Products = listOfProducts,
                Total = listOfProducts.Count()
            }; 

            return View(vm);
        }


        [HttpPost]
        public IActionResult Create(ProductCreateViewModel vm)
        {
            //check if the data is valid
            if (ModelState.IsValid)
            {
                //map vm to model
                Product product = new Product
                {
                    Name = vm.Name
                };
                //call the service
                _productDataService.Create(product);
                //go back to Home/Index
                return RedirectToAction("Details", "Product");
            }
            //if invalid
            return View(vm);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            //call services
            Product product = _productDataService.GetSingle(c => c.ProductId == id);
            IEnumerable<Product> listOfProducts = _productDataService.GetAll();

            //map
            ProductEditViewModel vm = new ProductEditViewModel
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Products = listOfProducts,
                Total = listOfProducts.Count()
            };


            return View(vm);
        }


        [HttpPost]
        public IActionResult Edit(ProductEditViewModel vm)
        {
            if (ModelState.IsValid)
            {
                Product product = new Product
                {
                    ProductId = vm.ProductId,
                    Name = vm.Name
                };

                //call service
                _productDataService.Update(product);

                //go to admin page with list of products
                return RedirectToAction("Details", "Product");
            }

            //pass to the view
            return View(vm);
        }


        [HttpGet]
        public IActionResult Delete(int id)
        {
            _productDataService.Delete(new Product { ProductId = id });
            return RedirectToAction("Details", "Product");
        }

        [HttpGet]
        public IActionResult Details()
        {
            //call service
            IEnumerable<Product> listOfProducts = _productDataService.GetAll();

            //vm
            ProductDetailsViewModel vm = new ProductDetailsViewModel
            {
                Products = listOfProducts,
                Total = listOfProducts.Count()
            };


            //pass to the view
            return View(vm);
        }
    }
}