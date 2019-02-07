using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//
using System.ComponentModel.DataAnnotations;

namespace BiankaKorban_DiplomaProject.ViewModels
{
    public class AccountLoginViewModel
    {
        public int CustomerId { get; set; }

        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
        [Required(ErrorMessage = "Username field is required"), MaxLength(300, ErrorMessage = "Username cannot be longer than 300 characters and shorter than 3 charasters"),
        MinLength(3),]
        public string UserName { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe{ get; set; }

        public string ReturnUrl { get; set; }


    }
}
