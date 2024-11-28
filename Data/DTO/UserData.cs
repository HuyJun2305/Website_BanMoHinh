using Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTO
{
    public class UserData
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string? ImgUrl { get; set; }
        public string Password { get; set; }
        public IEnumerable<string> role { get; set; }

    }
}

