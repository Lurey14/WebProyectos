using ExamenWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace ExamenWeb.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Producto> Productos => Set<Producto>();
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}
