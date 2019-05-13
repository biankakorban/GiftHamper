using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BiankaKorban_DiplomaProject.Models
{
    public class Admin
    {
        
        public int AdminId { get; set; } //PK

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
	}
}
