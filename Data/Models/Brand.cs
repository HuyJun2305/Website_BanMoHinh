using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Data.Models
{
    public  class Brand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
        [JsonIgnore]
        public virtual ICollection<Product>? Products { get; set; }

    }
}
