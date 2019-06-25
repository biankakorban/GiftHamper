using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//
using BiankaKorban_DiplomaProject.Models;


namespace GrandeGift.ViewModels
{
	public class CartCheckoutViewModel
	{
		public Customer Customer { get; set; }
		public Hamper Hamper { get; set; }
		public List<Address> MyAddresses { get; set; }
		public List<OrderLine> OrderLineItems { get; set; }
		public int AddressId { get; set; }
	}
}
