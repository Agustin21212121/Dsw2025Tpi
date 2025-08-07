using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Domain.Entities
{
    public class OrderItem : EntityBase
    {
        public OrderItem() { }

        public OrderItem(Guid productId, Guid orderId, int quantity, decimal unitPrice)
        {
            ProductId = productId;
            OrderId = orderId;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }

        public Guid ProductId { get; set; }

        public Product? Product { get; set; }

        public Guid OrderId { get; set; }
        public Order? Order { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        // Calculado en tiempo de ejecución, no se guarda en base
        public decimal Subtotal => UnitPrice * Quantity;
    }
}
