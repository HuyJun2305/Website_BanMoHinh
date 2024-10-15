using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Address
    {
        public Guid Id { get; set; }
        public string District { get; set; }
        public string Commune { get; set; }
        public string AddressDetail { get; set; }
        public string Description { get; set; }
        public virtual ApplicationUser Account { get; set; }    

    } 
}
