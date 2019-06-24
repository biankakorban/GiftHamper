using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//
using BiankaKorban_DiplomaProject.Models;

namespace BiankaKorban_DiplomaProject.ViewModels
{
	

	public class FilterSearchViewModel
	

	{
		public IEnumerable<Hamper> Hampers { get; set; }
		public IEnumerable<Category> Categories { get; set; }
		public int CategoryId { get; set; }
		public string Image { get; set; }
		public double min { get; set; }
		public double max { get; set; }
	}
}
