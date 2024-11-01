using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Image
    {
        public  Guid Id { get; set; }
        public string URL { get; set; }
        public bool Status { get; set; }
        public Guid ProductId { get; set; }
        [JsonIgnore]
        public virtual Product? Product { get; set; }
    }
} 
