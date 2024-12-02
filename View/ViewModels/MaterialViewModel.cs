using Microsoft.AspNetCore.Mvc;
using Data.Models;

namespace View.ViewModels
{
    public class MaterialViewModel
    {
        public Material NewMaterial { get; set; }
        public IEnumerable<Material> Materials { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int RowsPerPage { get; set; }
    }
}
