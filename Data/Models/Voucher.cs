using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Voucher
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Condition { get; set; }
        public int Percent { get; set; }
        public int Stock { get; set; }
        public DateTime DayStart { get; set; }
        public DateTime DayEnd { get; set; }
        public bool Status { get; set; }
        public Guid AccountId { get; set; }
        public virtual  ApplicationUser Account { get; set; }
    }
}
