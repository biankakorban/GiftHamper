using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//
using BiankaKorban_DiplomaProject.Models;

namespace BiankaKorban_DiplomaProject.ViewModels
{
	public class HomeHamperListViewModel
	{
		public IEnumerable<Hamper> Hampers { get; set; }
		public string Image { get; set; }
		public bool Discontinue { get; set; }
	}
}
