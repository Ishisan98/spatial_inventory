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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProducts()
        {
            var products = await _context.Products
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
                    CreatedDate = p.created_date,
                    CreatedBy = p.created_by,
                    ModifiedDate = p.modified_date,
                    ModifiedBy = p.modified_by
                })
                .ToListAsync();

            return Ok(products);
        }


        // get all active products
        [HttpGet("active-products")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllActiveProducts()
        {
            var activeProducts = await _context.Products
                .Where(p => p.status == "Active")
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
                    CreatedDate = p.created_date,
                    CreatedBy = p.created_by,
                    ModifiedDate = p.modified_date,
                    ModifiedBy = p.modified_by
                })
                .ToListAsync();

            return Ok(activeProducts);
        }


        // get a product by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProductById(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.product_id == id);

            if (product == null)
            {
                return NotFound();
            }

            var productDto = new ProductDto
            {
                ProductId = product.product_id,
                ProductName = product.product_name,
                Description = product.description,
                MeasuringUnit = product.measuring_unit,
                Quantity = product.quantity,
                MinQuantity = product.min_quantity,
                Price = product.unit_price,
                CategoryId = product.categoryId,
                Status = product.status,
                CreatedDate = product.created_date,
                CreatedBy = product.created_by,
                ModifiedDate = product.modified_date,
                ModifiedBy = product.modified_by
            };

            return Ok(productDto);
        }


        // get a product by Category ID
        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<List<ProductDto>>> GetProductsByCategoryId(int categoryId)
        {
            var activeProducts = await _context.Products
               .Where(p => p.categoryId == categoryId)
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
                categoryId = productDto.CategoryId
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

            var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.product_id == productDto.ProductId);
            if (existingProduct == null)
            {
                return NotFound($"Product with ID {productDto.ProductId} not found.");
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
        [HttpPut]
        public async Task<ActionResult<ProductDto>> DeactivateProduct(ProductDto productDto)
        {
            if (productDto == null || productDto.ProductId == 0)
            {
                return BadRequest("Invalid product data.");
            }

            var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.product_id == productDto.ProductId);
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
