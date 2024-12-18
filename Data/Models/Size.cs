﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Size
    {
        public Guid Id { get; set; }
        public string Value { get; set; }
        public bool Status { get; set; }
        public int Weight { get; set; }
        [JsonIgnore]
        public virtual ICollection<ProductSize>? ProductSizes { get; set; }
    }
}
