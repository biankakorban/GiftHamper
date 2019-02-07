using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//
using System.Collections.ObjectModel;
using BiankaKorban_DiplomaProject.Models;

namespace BiankaKorban_DiplomaProject.ViewModels
{
    public class HamperCreateViewModel
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public string Image { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public ICollection<Product> Products { get; set; }
        public int CategoryId { get; set; }
        public int Total { get; set; }
        public ICollection<Category> Categories { get; set; }

   

   
    }
}
