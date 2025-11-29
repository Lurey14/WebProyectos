using ExamenWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace ExamenWeb.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Producto> Productos => Set<Producto>();
        public DbSet<Categoria> Categorias => Set<Categoria>();
        public DbSet<Proveedor> Proveedores => Set<Proveedor>();
        public DbSet<Dragon> Dragons => Set<Dragon>();
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}
