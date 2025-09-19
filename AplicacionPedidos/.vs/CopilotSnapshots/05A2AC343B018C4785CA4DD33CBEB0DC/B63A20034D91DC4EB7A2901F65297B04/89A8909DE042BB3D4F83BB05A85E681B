using System.ComponentModel.DataAnnotations;

namespace AplicacionPedidos.Models
{
    public class OrderItemModel
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public OrderModel Order { get; set; }
        public int ProductoId { get; set; }
        public ProductModel Producto { get; set; }
        [Range(1, int.MaxValue)]
        public int Cantidad { get; set; }
        public decimal Subtotal { get; set; }
    }
}
