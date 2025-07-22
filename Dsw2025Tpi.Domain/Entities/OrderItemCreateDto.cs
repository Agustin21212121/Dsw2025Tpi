using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Domain.Entities
{
    public class OrderItemCreateDto
    {
        public Guid productId { get; set; }
        public int quantity { get; set; }
    }
}
