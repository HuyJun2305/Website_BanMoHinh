﻿using Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace View.ViewModels
{
    public class BrandViewModel
    {
        public Brand? Brand { get; set; }
        public IEnumerable<Brand>? brands { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int RowsPerPage { get; set; }
    }
}
