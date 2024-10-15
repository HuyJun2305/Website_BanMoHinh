using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public DateTime DayCreate { get; set; }
        public string Status { get; set; }
        public decimal Price { get; set; }

        public Guid? AccountId { get; set; }
        public Guid? VoucherId { get; set; }
        [JsonIgnore]
        public virtual Voucher? Voucher { get; set; }
        public virtual ApplicationUser? Account { get; set; }
    }
}
