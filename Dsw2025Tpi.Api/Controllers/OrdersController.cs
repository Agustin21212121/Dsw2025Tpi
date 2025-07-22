using Dsw2025Tpi.Data;
using Dsw2025Tpi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Dsw2025Tpi.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly Dsw2025TpiContext _context;

        public OrdersController(Dsw2025TpiContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult CreateOrder([FromBody] OrderCreateDto orderDto)
        {
            var customer = _context.Customers.Find(orderDto.customerId);
            if (customer == null)
                return BadRequest("Cliente no encontrado.");

            var orderItems = new List<OrderItem>();
            decimal total = 0;

            foreach (var itemDto in orderDto.items)
            {
                var product = _context.Products.Find(itemDto.productId);
                if (product == null)
                    return BadRequest($"Producto con ID {itemDto.productId} no encontrado.");

                if (!product.isActive)
                    return BadRequest($"El producto {product.name} está inhabilitado.");

                if (product.stockQuantity < itemDto.quantity)
                    return BadRequest($"No hay suficiente stock de {product.name}.");

                var subtotal = itemDto.quantity * product.currentUnitPrice;

                var orderItem = new OrderItem
                {
                    productId = product.Id,
                    quantity = itemDto.quantity,
                    unitPrice = product.currentUnitPrice,
                    subtotal = subtotal
                };

                product.stockQuantity -= itemDto.quantity;
                orderItems.Add(orderItem);
                total += subtotal;
            }

            var order = new Order
            {
                customerId = orderDto.customerId,
                shippingAddress = orderDto.shippingAddress,
                billingAddress = orderDto.billingAddress,
                notes = orderDto.notes,
                date = DateTime.Now,
                status = OrderStatus.PENDING,
                totalAmount = total,
                orderItems = orderItems
            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var orders = _context.Orders
                .Include(o => o.customer)
                .Include(o => o.orderItems)
                    .ThenInclude(oi => oi.product)
                .ToList();

            return Ok(orders);
        }


        // opcional: para que funcione CreatedAtAction
        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var order = _context.Orders
                .Include(o => o.orderItems)
                .ThenInclude(oi => oi.product)
                .Include(o => o.customer)
                .FirstOrDefault(o => o.Id == id);

            if (order == null)
                return NotFound();

            return Ok(order);
        }

        [HttpPatch("{id}/status")]
        public IActionResult UpdateOrderStatus(Guid id, [FromBody] UpdateOrderStatusDto dto)
        {
            var order = _context.Orders.Find(id);
            if (order == null)
                return NotFound("Orden no encontrada.");

            if (!Enum.TryParse<OrderStatus>(dto.status, true, out var newStatus))
                return BadRequest("Estado inválido. Usá: PENDING, PROCESSING, SHIPPED, DELIVERED o CANCELLED");

            order.status = newStatus;
            _context.SaveChanges();

            return Ok($"Estado de la orden actualizado a: {order.status}");
        }


    }

}

