using System.ComponentModel.DataAnnotations;

namespace View.ViewModels
{
    public class Sign_In_Up_ViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
    }
    
}
