using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//
using BiankaKorban_DiplomaProject.Models;

namespace BiankaKorban_DiplomaProject.Models
{
	public class Order
	{
		public int OrderId { get; set; } //PK
		public int CustomerId { get; set; } //FK
		public DateTime OrderDate { get; set; }

		public ICollection<OrderLine> OrderLines { get; set; }
	}
}
