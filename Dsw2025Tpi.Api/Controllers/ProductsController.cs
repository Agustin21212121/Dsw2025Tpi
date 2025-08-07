using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Dsw2025Tpi.Application.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace Dsw2025Tpi.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductsManagementService _service;

        public ProductsController(ProductsManagementService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductModel.ProductRequest request)
        {
            var response = await _service.CreateProduct(request);
            return CreatedAtAction(nameof(GetProductById), new { id = response.Id }, response);
        }   

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var products = await _service.GetProducts();
            return Ok(products);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            var product = await _service.GetProductById(id);
            return Ok(product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] ProductModel.ProductRequest request)
        {
            var updated = await _service.UpdateProduct(id, request);
            return Ok(updated);
        }


        [HttpPatch("{id}")]
        public async Task<IActionResult> Deactivate(Guid id)
        {
            var result = await _service.DeactivateProduct(id);
            return NoContent();
        }


    }
}
