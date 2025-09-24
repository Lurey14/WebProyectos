using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AplicacionPedidos.Models
{
    public class OrderItemModel
    {
        public int Id { get; set; }
        
        [Display(Name = "Pedido")]
        public int OrderId { get; set; }
        
        [ForeignKey("OrderId")]
        public OrderModel Order { get; set; }
        
        [Display(Name = "Producto")]
        [Required(ErrorMessage = "El producto es obligatorio")]
        public int ProductoId { get; set; }
        
        [ForeignKey("ProductoId")]
        public ProductModel Producto { get; set; }
        
        [Display(Name = "Cantidad")]
        [Required(ErrorMessage = "La cantidad es obligatoria")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser al menos 1")]
        public int Cantidad { get; set; }
        
        [Display(Name = "Subtotal")]
        [Column(TypeName = "decimal(18, 2)")]
        [DataType(DataType.Currency)]
        public decimal Subtotal { get; set; }
        
        public void CalcularSubtotal()
        {
            if (Producto != null)
            {
                Subtotal = Producto.Precio * Cantidad;
            }
        }
        
        [Display(Name = "Precio Unitario")]
        [Column(TypeName = "decimal(18, 2)")]
        [DataType(DataType.Currency)]
        public decimal PrecioUnitario { get; set; }
    }
}
