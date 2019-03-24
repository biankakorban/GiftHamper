using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BiankaKorban_DiplomaProject.Models
{
	public class Category
	{
		public int CategoryId { get; set; } //PK

		[Required]
		public string Name { get; set; }

	
		public ICollection<Hamper> Hampers { get; set; }
	}
}
