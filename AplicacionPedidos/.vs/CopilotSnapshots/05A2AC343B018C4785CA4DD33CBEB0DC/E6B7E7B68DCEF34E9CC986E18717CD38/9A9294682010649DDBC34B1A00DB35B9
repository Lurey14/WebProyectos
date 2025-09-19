using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AplicacionPedidos.Models
{
    public class OrderModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public UserModel Cliente { get; set; }
        public DateTime Fecha { get; set; }
        public string Estado { get; set; }
        public decimal Total { get; set; }
        public List<OrderItemModel> Items { get; set; }
    }
}