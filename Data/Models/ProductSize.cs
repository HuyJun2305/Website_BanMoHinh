using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Data.Models
{
    public class ProductSize
    {
        public Guid ProductId { get; set; }
        [JsonIgnore]
        public virtual Product? Product { get; set; }

        public Guid SizeId { get; set; }
        public virtual Size? Size { get; set; }
        public int Stock { get; set; }
    }

}
    