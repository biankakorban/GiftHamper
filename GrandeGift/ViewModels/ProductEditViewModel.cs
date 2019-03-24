using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
//
using BiankaKorban_DiplomaProject.Models;

namespace BiankaKorban_DiplomaProject.ViewModels
{
    public class ProductEditViewModel
    {
        public int ProductId { get; set; }

		[Required]
        public string Name { get; set; }
        public int Total { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}
