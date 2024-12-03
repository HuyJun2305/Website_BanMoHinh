using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Address
    {
        public Guid Id { get; set; }
        public string provinces { get; set; }
        public string District { get; set; }
        public string wards { get; set; }
        public string? AddressDetail { get; set; }
        public string? Description { get; set; }
        [ForeignKey("Account")]
        public Guid AccountId { get; set; }
        [JsonIgnore]
        public virtual ApplicationUser? Account { get; set; }    

    } 
}
