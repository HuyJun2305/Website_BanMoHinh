using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public decimal Price { get; set; }
        public string Name { get; set; }    
        public string Description { get; set; }
        public Guid IdBrand { get; set; }
        public Guid IdMaterial { get; set; }
        public Guid IdPromotion { get; set; }

        public Guid IdSize { get; set; }
        public virtual Size Size { get; set; }
        public virtual Brand Brand { get; set; }
        public virtual Material Material { get; set; }
        public virtual  Promotion? Promotion { get; set; }

    }
}
