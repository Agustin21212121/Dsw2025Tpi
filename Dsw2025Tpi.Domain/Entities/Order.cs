using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Domain.Entities
{
    public class Order : EntityBase
    {
        public DateTime date { get; set; }
        public string shippingAddress { get; set; }
        public string billingAddress { get; set; }
        public string notes { get; set; }
        public decimal totalAmount { get; set; }
        public OrderStatus status { get; set; } = OrderStatus.PENDING;


        //relacion con el cliente
        public Guid customerId { get; set; }
        public Customer customer { get; set; }

        //relacon con los items
        public ICollection<OrderItem> orderItems { get; set; }
    }
}
