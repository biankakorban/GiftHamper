using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//
using BiankaKorban_DiplomaProject.Models;
using BiankaKorban_DiplomaProject.ViewModels;

namespace BiankaKorban_DiplomaProject.ViewModels
{
    public class HamperDetailsViewModel
    {
		public ICollection<Hamper> Hampers { get; set; }
		public int HamperId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Image { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public List<ProductDetailsViewModel> ProductList { get; set; }
        public int CategoryId { get; set; }
        public string  CategoryName { get; set; }
        public ICollection<Category> Categories { get; set; }
        public List<HamperProductDetailsViewModel> HamperProductList { get; set; }
    }
}
