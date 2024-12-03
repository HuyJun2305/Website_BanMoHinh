using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public  class Promotion
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal? PriceReduced { get; set; }

        public int? PercentReduced { get; set; }
        public string Description { get; set; }
        public DateTime DayStart { get; set; }
        public DateTime DayEnd { get; set; }
        public bool Status { get; set; }
    }
}
