﻿using System;
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
        public int Stock { get; set; }
        public string? Description { get; set; }


        public Guid BrandId { get; set; }
        public virtual Brand? Brand { get; set; }
        public Guid MaterialId { get; set; }
        public virtual Material? Material { get; set; }
        public Guid CategoryId { get; set; }

        public virtual Category? Category { get; set; }


        public Guid? PromotionId { get; set; }
        [JsonIgnore]
        public virtual ICollection<Promotion>? Promotions { get; set; }
        public virtual ICollection<Image>? Images { get; set; }

        //[JsonIgnore]
        public virtual ICollection<ProductSize>? ProductSizes { get; set; }
    }
}
