using Microsoft.EntityFrameworkCore;
using AplicacionPedidos.Models;

namespace AplicacionPedidos.Data
{
    public class DBPedidosContext : DbContext
    {
        public DBPedidosContext(DbContextOptions<DBPedidosContext> options) : base(options) { }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<OrderModel> Orders { get; set; }
        public DbSet<OrderItemModel> OrderItems { get; set; }
    }
}
