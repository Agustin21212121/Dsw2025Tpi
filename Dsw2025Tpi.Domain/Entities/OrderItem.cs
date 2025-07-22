using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Domain.Entities
{
    public class OrderItem : EntityBase
    {
        public int quantity { get; set; }
        public decimal unitPrice { get; set; }
        public decimal subtotal { get; set; }

        //relacion con la orden
        public Guid orderId { get; set; }

        [JsonIgnore]
        public Order order { get; set; }

        //relacion con el producto
        public Guid productId { get; set; }
        public Product product { get; set; }

    }
}
