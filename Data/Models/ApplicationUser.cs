using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Data.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string? ImgUrl { get; set; }

        [JsonIgnore]
        public virtual ICollection<Address>? Addresses { get; set; }
		public virtual Cart Cart { get; set; }
    }
}
