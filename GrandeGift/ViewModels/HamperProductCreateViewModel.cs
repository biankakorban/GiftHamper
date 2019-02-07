using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//
using BiankaKorban_DiplomaProject.Models;

namespace BiankaKorban_DiplomaProject.ViewModels
{
    public class HamperProductCreateViewModel
    {
        public int HamperId { get; set; }
        public Hamper Hamper { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public IEnumerable<Category> CategoryList { get; set; }
        public IEnumerable<Hamper> HamperList { get; set; }
		public int ProductId { get; set; }
        public Product Product { get; set; }
		public IEnumerable<Product> ProductList { get; set; }
		public int TotalHamper { get; set; }
		public int TotalProduct { get; set; }
        public string Image { get; set; }
    }
}
