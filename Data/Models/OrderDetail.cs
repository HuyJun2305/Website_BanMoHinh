using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Data.Models
{
    public class OrderDetail
    {
        public Guid Id { get; set; }
        public decimal TotalPrice { get; set; }
        public int Quality { get; set; }
        public Guid OrderId { get; set; }
        [JsonIgnore]
        public virtual Order? Order { get; set; }
        public virtual Product? Product { get; set; }
    }
}
