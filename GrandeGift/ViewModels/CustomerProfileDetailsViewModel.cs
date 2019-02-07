using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//
using BiankaKorban_DiplomaProject.Models;
using System.ComponentModel.DataAnnotations;

namespace BiankaKorban_DiplomaProject.ViewModels
{
    public class CustomerProfileDetailsViewModel
    {
        public int CustomerId { get; set; } //PK

      
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
        [Required]
        public string Password { get; set; }
        public List<CustomerAddressDetailsViewModel> MyAddresses { get; set; }
    }
}
