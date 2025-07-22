using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Domain.Entities
{
    public class OrderCreateDto
    {
        public Guid customerId { get; set; }
        public string shippingAddress { get; set; }
        public string billingAddress { get; set; }
        public string notes { get; set; }
        public List<OrderItemCreateDto> items { get; set; }
    }
}
