using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Data.Models
{
    public class CartDetail
    {
        public Guid Id { get; set; }
        public decimal TotalPrice { get; set; }
        public int Quantity { get; set; }

        public Guid CartId { get; set; }
        [JsonIgnore]
        public virtual Cart? Cart { get; set; }

        public Guid ProductId { get; set; }
        [JsonIgnore]

        public virtual Product? Product { get; set; }

        public Guid SizeId { get; set; }
        [JsonIgnore]
        public virtual Size? Size { get; set; }
    }
}
