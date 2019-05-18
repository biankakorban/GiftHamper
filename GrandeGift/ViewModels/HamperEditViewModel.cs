using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using BiankaKorban_DiplomaProject.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BiankaKorban_DiplomaProject.ViewModels
{
	public class HamperEditViewModel
	{
		public int HamperId { get; set; }
		
		public string Name { get; set; }

		
		[DataType(DataType.Currency)]
		public double Price { get; set; }


		public string Image { get; set; }

		public int ProductId { get; set; }

		public Product Product { get; set; }

		public ICollection<Product> Products { get; set; }

		public int CategoryId { get; set; }


	
		public List<Hamper> HamperList { get; set; }

		//public bool checkboxProduct { get; set; }
		public string WhatTheHeck { get; set; }
		public bool Discontinue { get; set; }
	}
}
