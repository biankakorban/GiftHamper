using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace BiankaKorban_DiplomaProject.ViewModels
{
    public class CustomerProfileEditViewModel
    {
      
        public int CustomerId { get; set; }
    
        public string UserId { get; set; }

        public string UserName { get; set; }

		public string Password { get; set; }

		public string ConfirmPassword { get; set; }

		public string Email { get; set; }

		public string FirstName { get; set; }
      
        public string LastName { get; set; }
 
        public string Phone { get; set; }
		public IdentityUser User { get; set; }
		public List<CustomerAddressEditViewModel> MyAddresses { get; set; }

	}
}
