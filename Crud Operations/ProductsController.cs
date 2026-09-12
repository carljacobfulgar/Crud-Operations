using Microsoft.AspNetCore.Mvc;
using ProductApi.Models;

namespace ProductApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        // In-memory store for demonstration purposes
        private static readonly List<Product> Products = new()
        {
            new Product { ProductId = 1, Code = "P100", Name = "Keyboard", Description = "Mechanical keyboard", Price = 49.99m },
            new Product { ProductId = 2, Code = "P200", Name = "Mouse", Description = "Wireless mouse", Price = 25.50m }
        };

        // 1. GET (Read All): api/products
        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetAll()
        {
            return Ok(Products);
        }

        // 2. GET (Read by ID): api/products/{id}
        [HttpGet("{id}")]
        public ActionResult<Product> GetById(int id)
        {
            var product = Products.FirstOrDefault(p => p.ProductId == id);
            if (product == null)
            {
                return NotFound($"Product with ID {id} was not found.");
            }
            return Ok(product);
        }

        // 3. POST (Create): api/products
        [HttpPost]
        public ActionResult<Product> Create([FromBody] Product newProduct)
        {
            newProduct.ProductId = Products.Any() ? Products.Max(p => p.ProductId) + 1 : 1;
            Products.Add(newProduct);
            return CreatedAtAction(nameof(GetById), new { id = newProduct.ProductId }, newProduct);
        }

        // 4. PUT (Update): api/products/{id}
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Product updatedProduct)
        {
            var existingProduct = Products.FirstOrDefault(p => p.ProductId == id);
            if (existingProduct == null)
            {
                return NotFound($"Product with ID {id} was not found.");
            }

            existingProduct.Code = updatedProduct.Code;
            existingProduct.Name = updatedProduct.Name;
            existingProduct.Description = updatedProduct.Description;
            existingProduct.Price = updatedProduct.Price;

            return NoContent();
        }

        // 5. DELETE (Delete): api/products/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var product = Products.FirstOrDefault(p => p.ProductId == id);
            if (product == null)
            {
                return NotFound($"Product with ID {id} was not found.");
            }

            Products.Remove(product);
            return NoContent();
        }
    }
}