using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Domain.Entities
{
    public class Order : EntityBase
    {
        public Order() { }

        public Order(Guid customerId, string shippingAddress, string billingAddress)
        {
            CustomerId = customerId;
            Date = DateTime.UtcNow;
            ShippingAddress = shippingAddress;
            BillingAddress = billingAddress;
            Status = OrderStatus.PENDING;
        }

        public DateTime Date { get; set; }
        public string ShippingAddress { get; set; }
        public string BillingAddress { get; set; }
        public string? Notes { get; set; }
        public OrderStatus Status { get; set; }
        public Guid CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public decimal TotalAmount => OrderItems?.Sum(i => i.Subtotal) ?? 0m;
    }
}
