using Microsoft.EntityFrameworkCore;
using Data.Models;

namespace View.Database
{
    public class ViewContext : DbContext
    {
        public ViewContext(DbContextOptions<ViewContext> options) : base(options)
        {
        }
        public DbSet<Data.Models.Promotion>? Promotion { get; set; }
        public DbSet<Data.Models.Voucher>? Voucher { get; set; }
    }
}
