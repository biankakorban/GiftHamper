using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//
using BiankaKorban_DiplomaProject.Models;

namespace BiankaKorban_DiplomaProject.ViewModels
{
    public class ProductDetailsViewModel
    {
        public int Total { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
    }
}
