using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AplicacionPedidos.Models
{
    public class OrderModel
    {
        public int Id { get; set; }
        
        [Display(Name = "Cliente")]
        [Required(ErrorMessage = "El cliente es obligatorio")]
        public int UserId { get; set; }
        
        [ForeignKey("UserId")]
        [Display(Name = "Cliente")]
        public UserModel Cliente { get; set; }
        
        [Display(Name = "Fecha")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "La fecha es obligatoria")]
        public DateTime Fecha { get; set; } = DateTime.Now;
        
        [Display(Name = "Estado")]
        [Required(ErrorMessage = "El estado es obligatorio")]
        public string Estado { get; set; } = "Pendiente";
        
        [Display(Name = "Total")]
        [Column(TypeName = "decimal(18, 2)")]
        [DataType(DataType.Currency)]
        public decimal Total { get; set; }
        
        [Display(Name = "Dirección de Entrega")]
        [Required(ErrorMessage = "La dirección de entrega es obligatoria")]
        [StringLength(200, ErrorMessage = "La dirección no puede tener más de 200 caracteres")]
        public string DireccionEntrega { get; set; }
        
        [Display(Name = "Notas")]
        [StringLength(500)]
        public string Notas { get; set; }
        
        public List<OrderItemModel> Items { get; set; } = new List<OrderItemModel>();
        
        [NotMapped]
        public static List<string> EstadosDisponibles => new List<string>
        {
            "Pendiente",
            "Procesando",
            "Enviado",
            "Entregado",
            "Cancelado"
        };
        
        public void CalcularTotal()
        {
            Total = 0;
            if (Items != null)
            {
                foreach (var item in Items)
                {
                    item.CalcularSubtotal();
                    Total += item.Subtotal;
                }
            }
        }
        
        public bool VerificarStock(Dictionary<int, int> stockDisponible)
        {
            if (Items == null) return true;
            
            foreach (var item in Items)
            {
                if (!stockDisponible.ContainsKey(item.ProductoId) || stockDisponible[item.ProductoId] < item.Cantidad)
                {
                    return false;
                }
            }
            return true;
        }
        
        [NotMapped]
        public string ColorEstado
        {
            get
            {
                return Estado switch
                {
                    "Pendiente" => "#AAA2B1",
                    "Procesando" => "#4D6489",
                    "Enviado" => "#78809D",
                    "Entregado" => "#1C3150",
                    "Cancelado" => "#DBC6C0",
                    _ => "#AAA2B1",
                };
            }
        }
    }
}