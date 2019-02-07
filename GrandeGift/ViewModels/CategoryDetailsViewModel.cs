using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//
using BiankaKorban_DiplomaProject.Models;


namespace BiankaKorban_DiplomaProject.ViewModels
{
	public class CategoryDetailsViewModel
	{
		public int Total { get; set; }
		public IEnumerable<Category> Categories { get; set; }
	}
}
