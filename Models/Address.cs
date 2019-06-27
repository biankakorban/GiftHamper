using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//
using System.ComponentModel.DataAnnotations;

namespace BiankaKorban_DiplomaProject.Models
{
    public class Address
    {
        public int AddressId { get; set; } //PK

        [Required]

        public string Line1 { get; set; }
		
        public string Line2 { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string PostalCode { get; set; }
		[Required]
		public string Suburb { get; set; }

		public int CustomerId { get; set; } //FK


	}
}
