using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using spatial_inventory_server.Data;
using spatial_inventory_server.Models;


namespace spatial_inventory_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public ProductController(ApiDbContext context) {
            _context = context;
        }


        // get all products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
        {
            return await _context.Products.ToListAsync();
        }


        // get all active products
        [HttpGet("active-products")]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllActiveProducts()
        {
            var activeProducts = await _context.Products
                                     .Where(p => p.status == "Active")
                                     .ToListAsync();

            return Ok(activeProducts);
        }


        // get a product by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetProductById(int id)
        {
            var product = await _context.Categories.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return product;
        }


        // get a product by Category ID
        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<List<Product>>> GetProductsByCategoryId(int categoryId)
        {
            var products = await _context.Products
                                         .Where(p => p.category_id == categoryId)
                                         .ToListAsync();

            if (products == null || !products.Any())
            {
                return NotFound();
            }

            return products;
        }


        // create a new product
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProductById), new { id = product.product_id }, product);
        }


        // update product
        [HttpPut]
        public async Task<ActionResult<Product>> UpdateProduct(Product product)
        {
            if (product == null || product.product_id == 0)
            {
                return BadRequest("Invalid product data.");
            }

            var existingProduct = await _context.Products.FindAsync(product.product_id);
            if (existingProduct == null)
            {
                return NotFound($"Product with ID {product.product_id} not found.");
            }

            existingProduct.product_name = product.product_name;
            existingProduct.description = product.description;
            existingProduct.price = product.price;
            existingProduct.measuring_unit = product.measuring_unit;
            existingProduct.quantity = product.quantity;
            existingProduct.category_id = product.category_id;
            existingProduct.modified_by = product.modified_by;
            existingProduct.modified_date = DateTime.Now;

            await _context.SaveChangesAsync();

            return Ok(existingProduct);
        }


        // deactivate product
        [HttpPut]
        public async Task<ActionResult<Product>> DeactivateProduct(Product product)
        {
            if (product == null || product.product_id == 0)
            {
                return BadRequest("Invalid product Id.");
            }

            var existingProduct = await _context.Products.FindAsync(product.product_id);
            if (existingProduct == null)
            {
                return NotFound($"Product with ID {product.product_id} not found.");
            }

            existingProduct.status = "Inactive";
            existingProduct.modified_by = product.modified_by;
            existingProduct.modified_date = DateTime.Now;

            await _context.SaveChangesAsync();

            return Ok(existingProduct);
        }
    }
}
