using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//
using System.ComponentModel.DataAnnotations;


namespace BiankaKorban_DiplomaProject.ViewModels
{
    public class AccountRegisterViewModel
    {
		[StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters")]
		[Required(ErrorMessage = "Username field is required"), MaxLength(300, ErrorMessage = "Username cannot be longer than 300 characters and shorter than 3 charasters"),
		MinLength(3), Display(Name = "Username")]
		public string UserName { get; set; }

        [Required, DataType(DataType.Password, ErrorMessage = "Wrong Password")]
        public string Password { get; set; }

        [Required, DataType(DataType.Password, ErrorMessage = "Wrong password"), Compare(nameof(Password)), 
			Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Firstname field is required"), MaxLength(256), Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Lastname field is required"), MaxLength(256), Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "The Phone number field is required")]
        public string Phone { get; set; }
    }
}
