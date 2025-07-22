using Dsw2025Tpi.Data;
using Dsw2025Tpi.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Dsw2025Tpi.Api.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly Dsw2025TpiContext _context;

        public ProductsController(Dsw2025TpiContext context)
        {
           _context = context;
        }
        [HttpGet]
        public IActionResult GetAll() 
        {
            var products = _context.Products.ToList();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var product = _context.Products.Find(id);

            if (product == null)
                return NotFound("Producto no encontrado.");

            return Ok(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult CreateProduct([FromBody] Product product)
        {

            if (string.IsNullOrWhiteSpace(product.sku) || string.IsNullOrWhiteSpace(product.name))
                return BadRequest("Se requieren SKU y nombre.");

            var skuExists = _context.Products.Any(p => p.sku == product.sku);
            if (skuExists)
                return BadRequest("El SKU ya existe.");

            var codeExists = _context.Products.Any(p => p.internalCode == product.internalCode);
            if (codeExists)
                return BadRequest("El código interno ya existe.");


            _context.Products.Add(product);
           _context.SaveChanges();
            return CreatedAtAction(nameof(GetAll), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProduct(Guid id, [FromBody] Product updatedProduct)
        {
            var existingProduct = _context.Products.Find(id);

            if (existingProduct == null)
                return NotFound("Producto no encontrado.");

            if (string.IsNullOrWhiteSpace(updatedProduct.sku) || string.IsNullOrWhiteSpace(updatedProduct.name))
                return BadRequest("Se requieren SKU y nombre.");

            var skuExists = _context.Products.Any(p => p.sku == updatedProduct.sku && p.Id != id);
            if (skuExists)
                return BadRequest("Otro producto ya usa ese SKU.");

            // Actualizar propiedades
            existingProduct.sku = updatedProduct.sku;
            existingProduct.internalCode = updatedProduct.internalCode;
            existingProduct.name = updatedProduct.name;
            existingProduct.description = updatedProduct.description;
            existingProduct.currentUnitPrice = updatedProduct.currentUnitPrice;
            existingProduct.stockQuantity = updatedProduct.stockQuantity;
            existingProduct.isActive = updatedProduct.isActive;

            _context.SaveChanges();

            return Ok(existingProduct);
        }


        [HttpPatch("{id}/disable")]
        public IActionResult DisableProduct(Guid id)
        {
            var product = _context.Products.Find(id);

            if (product == null)
                return NotFound("Producto no encontrado.");

            if (!product.isActive)
                return BadRequest("El producto ya está inhabilitado.");

            product.isActive = false;
            _context.SaveChanges();

            return Ok("Producto inhabilitado correctamente.");
        }

        [HttpPatch("{id}/enable")]
        public IActionResult EnableProduct(Guid id)
        {
            var product = _context.Products.Find(id);

            if (product == null)
                return NotFound("Producto no encontrado.");

            if (product.isActive)
                return BadRequest("El producto ya está habilitado.");

            product.isActive = true;
            _context.SaveChanges();

            return Ok("Producto habilitado correctamente.");
        }





    }
}
