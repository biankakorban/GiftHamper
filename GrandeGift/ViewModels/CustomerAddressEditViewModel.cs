using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//
using BiankaKorban_DiplomaProject.Models;
using System.ComponentModel.DataAnnotations;

namespace BiankaKorban_DiplomaProject.ViewModels
{
    public class CustomerAddressEditViewModel
    {
        public int AddressId { get; set; }

		[Required, Display(Name = "Street")]
		public string Line1 { get; set; }

		[Display(Name = "Street")]
		public string Line2 { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string Country { get; set; }
        [Required, Display (Name = "Postal Code")]
        public string PostalCode { get; set; }
        [Required]
        public string Suburb { get; set; }
    }
}
