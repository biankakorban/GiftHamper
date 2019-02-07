using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//
using System.ComponentModel.DataAnnotations;

namespace BiankaKorban_DiplomaProject.ViewModels
{
    public class HomeIndexViewModel
    {
        [Required(ErrorMessage = "Firstname field is required"), MaxLength(256)]
        public string FirstName { get; set; }
        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
