using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//
using BiankaKorban_DiplomaProject.Models;

namespace BiankaKorban_DiplomaProject.ViewModels
{
	public class HamperProductDetailsViewModel
	{
		public int HamperId { get; set; }
		public Hamper Hamper { get; set; }
		public IEnumerable<Hamper> HamperList { get; set; }

		public int ProductId { get; set; }
		public Product Product { get; set; }
		public IEnumerable<Product> ProductList { get; set; }
	}
}
