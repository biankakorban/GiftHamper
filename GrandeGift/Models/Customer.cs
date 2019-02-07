using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BiankaKorban_DiplomaProject.Models
{
    public class Customer
    {
        public int CustomerId { get; set; } //PK

        [Required]
        public string UserId { get; set; } //FK

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
       

        //collection of customer addresses
        public ICollection<Address> MyAddresses { get; set; }
		public ICollection<Order> MyOrders { get; set; }
		public ICollection<Review> Review { get; set; }

	}
}
