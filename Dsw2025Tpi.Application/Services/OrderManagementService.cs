using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Application.Exceptions;
using Dsw2025Tpi.Domain.Entities;
using Dsw2025Tpi.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Application.Services
{
    public class OrderManagementService
    {
        private readonly IRepository _repository;

        public OrderManagementService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<OrderModel.OrderResponse> CreateOrder(OrderModel.OrderRequest request)
        {
            if (request.CustomerId == Guid.Empty)
                throw new ArgumentException("CustomerId es obligatorio.");

            if (string.IsNullOrWhiteSpace(request.ShippingAddress))
                throw new ArgumentException("La dirección de envío es obligatoria.");

            if (string.IsNullOrWhiteSpace(request.BillingAddress))
                throw new ArgumentException("La dirección de facturación es obligatoria.");

            if (request.OrderItems == null || !request.OrderItems.Any())
                throw new ArgumentException("Debe incluir al menos un item en la orden.");

            foreach (var item in request.OrderItems)
            {
                if (item.ProductId == Guid.Empty)
                    throw new ArgumentException("Se encontró un item sin ProductId.");

                if (item.Quantity <= 0)
                    throw new ArgumentException("La cantidad debe ser mayor que cero.");
            }

            // Cliente
            var customer = await _repository.GetById<Customer>(request.CustomerId);
            if (customer == null)
                throw new EntityNotFoundException("El cliente no existe.");

            // Crear orden
            var order = new Order(
                customerId: request.CustomerId,
                shippingAddress: request.ShippingAddress,
                billingAddress: request.BillingAddress
            );

            // Procesar items
            foreach (var item in request.OrderItems)
            {
                var product = await _repository.GetById<Product>(item.ProductId);
                if (product == null)
                    throw new EntityNotFoundException($"Producto con ID {item.ProductId} no encontrado.");

                if (product.StockQuantity < item.Quantity)
                    throw new ArgumentException($"No hay suficiente stock para el producto '{product.Name}'.");

                product.StockQuantity -= item.Quantity;
                await _repository.Update(product);

                var orderItem = new OrderItem(
                    productId: product.Id,
                    orderId: order.Id,
                    quantity: item.Quantity,
                    unitPrice: product.CurrentUnitPrice
                );

                order.OrderItems.Add(orderItem);
            }

            await _repository.Add(order);

            return new OrderModel.OrderResponse(
                OrderId: order.Id,
                CustomerId: customer.Id,
                Date: order.Date,
                ShippingAddress: order.ShippingAddress,
                BillingAddress: order.BillingAddress,
                Notes: order.Notes,
                TotalAmount: order.TotalAmount,
                Status: order.Status.ToString(),
                OrderItems: order.OrderItems.Select(oi => new OrderItemModel.OrderItemResponse(
                    Id: oi.Id,
                    ProductId: oi.ProductId,
                    ProductName: oi.Product?.Name ?? "",
                    Quantity: oi.Quantity,
                    UnitPrice: oi.UnitPrice,
                    Subtotal: oi.Subtotal
                ))
            );
        }

        public async Task<OrderModel.OrderResponse?> GetOrderById(Guid id)
        {
            var order = await _repository.GetById<Order>(id, "OrderItems", "OrderItems.Product");

            if (order == null)
                return null;

            return new OrderModel.OrderResponse(
                OrderId: order.Id,
                CustomerId: order.CustomerId,
                Date: order.Date,
                ShippingAddress: order.ShippingAddress,
                BillingAddress: order.BillingAddress,
                Notes: order.Notes,
                TotalAmount: order.TotalAmount,
                Status: order.Status.ToString(),
                OrderItems: order.OrderItems.Select(oi => new OrderItemModel.OrderItemResponse(
                    Id: oi.Id,
                    ProductId: oi.ProductId,
                    ProductName: oi.Product?.Name ?? "",
                    Quantity: oi.Quantity,
                    UnitPrice: oi.UnitPrice,
                    Subtotal: oi.Subtotal
                ))
            );
        }

        public async Task<IEnumerable<OrderModel.OrderResponse>> GetAllOrders()
        {

            var orders = await _repository.GetAll<Order>("OrderItems", "OrderItems.Product");

            if (!orders.Any())
                throw new EntityNotFoundException("No hay órdenes registradas.");

            return orders.Select(order => new OrderModel.OrderResponse(
                OrderId: order.Id,
                CustomerId: order.CustomerId,
                Date: order.Date,
                ShippingAddress: order.ShippingAddress,
                BillingAddress: order.BillingAddress,
                Notes: order.Notes,
                TotalAmount: order.TotalAmount,
                Status: order.Status.ToString(),
                OrderItems: order.OrderItems.Select(oi => new OrderItemModel.OrderItemResponse(
                    Id: oi.Id,
                    ProductId: oi.ProductId,
                    ProductName: oi.Product?.Name ?? "",
                    Quantity: oi.Quantity,
                    UnitPrice: oi.UnitPrice,
                    Subtotal: oi.Subtotal
                ))
            ));
        }

        public async Task<OrderModel.OrderResponse> UpdateOrderStatus(Guid orderId, string newStatus)
        {
            var order = await _repository.GetById<Order>(orderId, "OrderItems", "OrderItems.Product");

            if (order == null)
                throw new EntityNotFoundException("Orden no encontrada.");

            if (!Enum.TryParse<OrderStatus>(newStatus, true, out var parsedStatus))
                throw new ArgumentException("Estado invalido.");

            order.Status = parsedStatus;
            await _repository.Update(order);

            return new OrderModel.OrderResponse(
                OrderId: order.Id,
                CustomerId: order.CustomerId,
                Date: order.Date,
                ShippingAddress: order.ShippingAddress,
                BillingAddress: order.BillingAddress,
                Notes: order.Notes,
                TotalAmount: order.TotalAmount,
                Status: order.Status.ToString(),
                OrderItems: order.OrderItems.Select(oi => new OrderItemModel.OrderItemResponse(
                    Id: oi.Id,
                    ProductId: oi.ProductId,
                    ProductName: oi.Product?.Name ?? "",
                    Quantity: oi.Quantity,
                    UnitPrice: oi.UnitPrice,
                    Subtotal: oi.Subtotal
                ))
            );
        }
    }
}

