using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using spatial_inventory_server.Data;
using spatial_inventory_server.Models;
using spatial_inventory_server.Dto;


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
        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProducts(int userId)
        {
            var products = await _context.Products
                .Where(p => p.userId == userId)
                .Select(p => new ProductDto
                {
                    ProductId = p.product_id,
                    ProductName = p.product_name,
                    Description = p.description,
                    MeasuringUnit = p.measuring_unit,
                    Quantity = p.quantity,
                    MinQuantity = p.min_quantity,
                    Price = p.unit_price,
                    CategoryId = p.categoryId,
                    Status = p.status,
                    UserId = p.userId,
                    CreatedDate = p.created_date,
                    CreatedBy = p.created_by,
                    ModifiedDate = p.modified_date,
                    ModifiedBy = p.modified_by
                })
                .ToListAsync();

            return Ok(products);
        }


        // get all active products
        [HttpGet("active-products/{userId}")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllActiveProducts(int userId)
        {
            var activeProducts = await _context.Products
                .Where(p => p.status == "Active" && p.userId == userId)
                .Select(p => new ProductDto
                {
                    ProductId = p.product_id,
                    ProductName = p.product_name,
                    Description = p.description,
                    MeasuringUnit = p.measuring_unit,
                    Quantity = p.quantity,
                    MinQuantity = p.min_quantity,
                    Price = p.unit_price,
                    CategoryId = p.categoryId,
                    Status = p.status,
                    UserId = p.userId,
                    CreatedDate = p.created_date,
                    CreatedBy = p.created_by,
                    ModifiedDate = p.modified_date,
                    ModifiedBy = p.modified_by
                })
                .ToListAsync();

            return Ok(activeProducts);
        }


        // get a product by ID
        [HttpGet("productById")]
        public async Task<ActionResult<ProductDto>> GetProductById(ProductDto product)
        {
            var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.product_id == product.ProductId && p.userId == product.UserId);

            if (existingProduct == null)
            {
                return NotFound();
            }

            var productDto = new ProductDto
            {
                ProductId = existingProduct.product_id,
                ProductName = existingProduct.product_name,
                Description = existingProduct.description,
                MeasuringUnit = existingProduct.measuring_unit,
                Quantity = existingProduct.quantity,
                MinQuantity = existingProduct.min_quantity,
                Price = existingProduct.unit_price,
                CategoryId = existingProduct.categoryId,
                Status = existingProduct.status,
                UserId = existingProduct.userId,
                CreatedDate = existingProduct.created_date,
                CreatedBy = existingProduct.created_by,
                ModifiedDate = existingProduct.modified_date,
                ModifiedBy = existingProduct.modified_by
            };

            return Ok(productDto);
        }


        // get a product by Category ID
        [HttpGet("productByCategory")]
        public async Task<ActionResult<List<ProductDto>>> GetProductsByCategoryId(CategoryDto category)
        {
            var activeProducts = await _context.Products
               .Where(p => p.categoryId == category.CategoryId && p.userId == category.UserId)
               .Select(p => new ProductDto
               {
                   ProductId = p.product_id,
                   ProductName = p.product_name,
                   Description = p.description,
                   MeasuringUnit = p.measuring_unit,
                   Quantity = p.quantity,
                   MinQuantity = p.min_quantity,
                   Price = p.unit_price,
                   CategoryId = p.categoryId,
                   Status = p.status,
                   UserId = p.userId,
                   CreatedDate = p.created_date,
                   CreatedBy = p.created_by,
                   ModifiedDate = p.modified_date,
                   ModifiedBy = p.modified_by
               })
               .ToListAsync();

            return Ok(activeProducts);
        }


        // create a new product
        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateProduct(ProductDto productDto)
        {
            var newProduct = new Product
            {
                product_name = productDto.ProductName,
                description = productDto.Description,
                measuring_unit = productDto.MeasuringUnit,
                quantity = productDto.Quantity,
                min_quantity = productDto.MinQuantity,
                unit_price = productDto.Price,
                categoryId = productDto.CategoryId,
                userId = productDto.UserId
            };
            try
            {
                _context.Products.Add(newProduct);
                await _context.SaveChangesAsync();
                return Ok(newProduct.product_id);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message, details = ex.InnerException?.Message });
            }
        }


        // update product
        [HttpPut]
        public async Task<ActionResult<ProductDto>> UpdateProduct(ProductDto productDto)
        {
            if (productDto == null || productDto.ProductId == 0)
            {
                return BadRequest("Invalid product data.");
            }

            var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.product_id == productDto.ProductId && p.userId == productDto.UserId);
            if (existingProduct == null)
            {
                return NotFound($"Product not found.");
            }

            existingProduct.product_name = productDto.ProductName;
            existingProduct.description = productDto.Description;
            existingProduct.measuring_unit = productDto.MeasuringUnit;
            existingProduct.quantity = productDto.Quantity;
            existingProduct.min_quantity = productDto.MinQuantity;
            existingProduct.unit_price = productDto.Price;
            existingProduct.categoryId = productDto.CategoryId;
            existingProduct.description = productDto.Description;
            existingProduct.modified_by = productDto.ModifiedBy;
            existingProduct.modified_date = DateTime.Now;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(existingProduct.product_id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // deactivate product
        [HttpPut("deactivate-product")]
        public async Task<ActionResult<ProductDto>> DeactivateProduct(ProductDto productDto)
        {
            if (productDto == null || productDto.ProductId == 0)
            {
                return BadRequest("Invalid product data.");
            }

            var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.product_id == productDto.ProductId && p.userId == productDto.UserId);
            if (existingProduct == null)
            {
                return NotFound($"Product with ID {productDto.ProductId} not found.");
            }

            existingProduct.status = "Inactive";
            existingProduct.modified_by = productDto.ModifiedBy;
            existingProduct.modified_date = DateTime.Now;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(existingProduct.product_id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
