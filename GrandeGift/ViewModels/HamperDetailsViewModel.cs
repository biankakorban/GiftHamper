using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
//
using BiankaKorban_DiplomaProject.Models;
using BiankaKorban_DiplomaProject.ViewModels;

namespace BiankaKorban_DiplomaProject.ViewModels
{
    public class HamperDetailsViewModel
    {
		
		public int HamperId { get; set; }
        public string Name { get; set; }

		[DisplayFormat(DataFormatString = "{0:C}")]
		public double Price { get; set; }

        public string Image { get; set; }
        public ICollection<Product> Products { get; set; }
		public List<Hamper> HamperList { get; set; }
        //public List<ProductDetailsViewModel> ProductList { get; set; }
        public int CategoryId { get; set; }
        public string  CategoryName { get; set; }
        public ICollection<Category> Categories { get; set; }

	}
}
