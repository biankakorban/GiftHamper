using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//
using BiankaKorban_DiplomaProject.Models;

namespace BiankaKorban_DiplomaProject.Models
{
	public class HamperProduct
	{
		
		public int HamperId { get; set; } //FK
		public Hamper hamper { get; set; }
		public int ProductId { get; set; } //FK
		public Product product { get; set; }
	}
}
