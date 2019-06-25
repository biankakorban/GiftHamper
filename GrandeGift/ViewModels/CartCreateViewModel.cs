using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//
using BiankaKorban_DiplomaProject.Models;

namespace GrandeGift.ViewModels
{
	public class CartCreateViewModel
	{
		public int CategoryId { get; set; }
		public IEnumerable<Hamper> Hampers { get; set; }
		public string Image { get; set; }
		public string CategoryName { get; set; }
	}
}
