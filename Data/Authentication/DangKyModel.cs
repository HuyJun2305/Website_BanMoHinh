using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Authentication
{
    public class Sign_In_Up_ViewModel
    {
        [Required]
        public  string Username { get; set; }
        [Required]
        public  string Password { get; set; }
        [Required,EmailAddress]
        public  string Email { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }

    }
}
