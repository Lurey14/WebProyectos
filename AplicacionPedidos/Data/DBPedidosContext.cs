using Microsoft.EntityFrameworkCore;
using AplicacionPedidos.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace AplicacionPedidos.Data
{
    public class DBPedidosContext : IdentityDbContext<ApplicationUser>
    {
        public DBPedidosContext(DbContextOptions<DBPedidosContext> options) : base(options) { }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<OrderModel> Orders { get; set; }
        public DbSet<OrderItemModel> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserModel>().ToTable("CustomUsers");

            modelBuilder.Entity<OrderItemModel>()
                .Property(oi => oi.Subtotal)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<OrderModel>()
                .Property(o => o.Total)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<ProductModel>()
                .Property(p => p.Precio)
                .HasColumnType("decimal(18,2)");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.ConfigureWarnings(warnings => 
                warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
        }
    }
}
