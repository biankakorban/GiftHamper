﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
//
using BiankaKorban_DiplomaProject.Models;

namespace BiankaKorban_DiplomaProject.Models
{
	public class Hamper
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int HamperId { get; set; } //PK
		public string Name { get; set; }
		public double Price { get; set; }
        //image property
        public string Image { get; set; }
        public int CategoryId { get; set; }
        public ICollection<HamperProduct> Products { get; set; }

		public ICollection<OrderLine> OrderLines { get; set; }
		public bool Discontinue { get; set; }








	}
}
