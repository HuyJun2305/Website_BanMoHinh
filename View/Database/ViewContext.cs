using Microsoft.EntityFrameworkCore;
using Data.Models;

namespace View.Database
{
    public class ViewContext : DbContext
    {
        public ViewContext(DbContextOptions<ViewContext> options) : base(options) 
        {
            
        }

        public DbSet<Data.Models.Brand>? Brand { get; set; }
        public DbSet<Data.Models.Image>? Image { get; set; }
        public DbSet<Data.Models.Product>? Product { get; set; }
        public DbSet<Data.Models.Size>? Size { get; set; }
        public DbSet<Data.Models.Material>? Material { get; set; }
        public DbSet<Data.Models.Promotion>? Promotion { get; set;}
    }
}
