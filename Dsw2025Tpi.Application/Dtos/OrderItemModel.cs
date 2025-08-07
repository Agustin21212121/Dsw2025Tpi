﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Application.Dtos
{
    public static class OrderItemModel
    {
        public record OrderItemRequest(
            Guid ProductId,
            int Quantity
        );

        public record OrderItemResponse(
             Guid Id,
            Guid ProductId,
            string ProductName,
            int Quantity,
            decimal UnitPrice,
            decimal Subtotal
        );
    }
}
