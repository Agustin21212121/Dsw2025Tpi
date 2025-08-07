using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Application.Exceptions;
using Dsw2025Tpi.Application.Services;
using Dsw2025Tpi.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dsw2025Tpi.Api.Controllers
{

    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
        public class OrdersController : ControllerBase
        {
            private readonly OrderManagementService _orderService;

            public OrdersController(OrderManagementService orderService)
            {
                _orderService = orderService;
            }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateOrder([FromBody] OrderModel.OrderRequest request)
            {
                var result = await _orderService.CreateOrder(request);
                return CreatedAtAction(nameof(GetOrderById), new { id = result.OrderId }, result);
            }

            [HttpGet("{id}")]
            public async Task<IActionResult> GetOrderById(Guid id)
            {
                var order = await _orderService.GetOrderById(id);
                return Ok(order);
            }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _orderService.GetAllOrders();
            return Ok(orders);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateOrderStatus(Guid id, [FromBody] UpdateStatusRequestModel request)
        {
            var result = await _orderService.UpdateOrderStatus(id, request.NewStatus);
            return Ok(result); 
        }


    }
    }


