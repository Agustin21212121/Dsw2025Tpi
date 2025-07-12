using Microsoft.AspNetCore.Mvc;

namespace Dsw2025Tpi.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        [HttpGet]

        public IActionResult GetProducts() 
        {
            return Ok(new List<string> { "Producto 1", "Producto 2" });
        
        }
        
    }
}
