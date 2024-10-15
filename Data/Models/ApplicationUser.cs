using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string ImgUrl { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }
        public virtual Cart Cart { get; set; }
    }
}
