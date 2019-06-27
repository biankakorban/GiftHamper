using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//
using BiankaKorban_DiplomaProject.Models;


namespace BiankaKorban_DiplomaProject.Models
{
	public class Review
	{
		public int ReviewId { get; set; } //PK
		public int CustomerId { get; set; } //FK

		public string Description { get; set; }
	}
}
