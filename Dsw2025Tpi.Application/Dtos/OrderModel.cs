using Dsw2025Tpi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Application.Dtos
{
    public static class OrderModel
    {
        public record OrderRequest(
            Guid CustomerId,
            string ShippingAddress,
            string BillingAddress,
            IEnumerable<OrderItemModel.OrderItemRequest> OrderItems

        );

        public record OrderResponse(
            Guid OrderId,
            Guid CustomerId,
            DateTime Date,
            string ShippingAddress,
            string BillingAddress,
            string Notes,
            decimal TotalAmount,
            string Status,
            IEnumerable<OrderItemModel.OrderItemResponse> OrderItems
        );
        
    }
}
