using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTO
{
    public class OrderDetailsDto
    {
        public string CustomerName { get; set; }
        public decimal ShippingFee { get; set; }
        public string Status { get; set; }
        public OrderAddressDto OrderAddress { get; set; }
    }

    public class OrderAddressDto
    {
        public string City { get; set; }
        public string District { get; set; }
        public string Ward { get; set; }
        public string AddressDetail { get; set; }
    }

}
