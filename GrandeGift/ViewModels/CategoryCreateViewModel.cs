using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//
using System.ComponentModel.DataAnnotations;
using BiankaKorban_DiplomaProject.Models;

namespace BiankaKorban_DiplomaProject.ViewModels
{
	public class CategoryCreateViewModel
	{
        [Display(Name = "Name of the category")]
        [Required (ErrorMessage = "Field cannot be empty")]
		public string Name { get; set; }


        public int Total { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}
