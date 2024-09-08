using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using spatial_inventory_server.Data;
using spatial_inventory_server.Dto;
using spatial_inventory_server.Models;


namespace Spatial_Inventory.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public CategoryController(ApiDbContext context)
        {
            _context = context;
        }


        // get all categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAllCategories()
        {
            var categories = await _context.Categories
                            .Select(c => new CategoryDto
                            {
                                CategoryId = c.category_id,
                                CategoryName = c.category_name,
                                Description = c.description,
                                Status = c.status,
                                CreatedDate = c.created_date,
                                CreatedBy = c.created_by,
                                ModifiedDate = c.modified_date,
                                ModifiedBy = c.modified_by
                            })
                            .ToListAsync();

            return Ok(categories);
        }


        // get all active categories
        [HttpGet("active-categories")]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAllActiveCategories()
        {
            var activeCategories = await _context.Categories
                            .Where(c => c.status == "Active")
                            .Select(c => new CategoryDto
                            {
                                CategoryId = c.category_id,         
                                CategoryName = c.category_name,     
                                Description = c.description,      
                                Status = c.status,                 
                                CreatedDate = c.created_date,     
                                CreatedBy = c.created_by,
                                ModifiedDate = c.modified_date,
                                ModifiedBy = c.modified_by
                            })
                            .ToListAsync();

            return Ok(activeCategories);
        }


        // get a category by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetCategoryById(int id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.category_id == id);

            if (category == null)
            {
                return NotFound();
            }

            var categoryDto = new CategoryDto
            {
                CategoryId = category.category_id,
                CategoryName = category.category_name,
                Description = category.description,
                Status = category.status,
                CreatedDate = category.created_date,
                CreatedBy = category.created_by,
                ModifiedDate = category.modified_date,
                ModifiedBy = category.modified_by
            };

            return Ok(categoryDto);
        }


        // create a new category
        [HttpPost]
        public async Task<ActionResult<CategoryDto>> CreateCategory(CategoryDto categoryDto)
        {
            var category = new Category
            {
                category_name = categoryDto.CategoryName,
                description = categoryDto.Description,
                created_by = categoryDto.CreatedBy,
                created_date = DateTime.Now
            };
            try
            {
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();

                return Ok(category.category_id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // update category
        [HttpPut]
        public async Task<ActionResult<CategoryDto>> UpdateCategory(CategoryDto categoryDto)
        {
            if (categoryDto == null || categoryDto.CategoryId == 0)
            {
                return BadRequest("Invalid category data.");
            }

            var existingCategory = await _context.Categories.FirstOrDefaultAsync(c => c.category_id == categoryDto.CategoryId);
            if (existingCategory == null)
            {
                return NotFound($"Category with ID {categoryDto.CategoryId} not found.");
            }

            existingCategory.category_name = categoryDto.CategoryName;
            existingCategory.description = categoryDto.Description;
            existingCategory.modified_by = categoryDto.ModifiedBy;
            existingCategory.modified_date = DateTime.Now;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(existingCategory.category_id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // deactivate category
        [HttpPut("deactivate-category")]
        public async Task<ActionResult<CategoryDto>> DeactivateCategory(CategoryDto categoryDto)
        {
            if (categoryDto == null || categoryDto.CategoryId == 0)
            {
                return BadRequest("Invalid category data.");
            }

            var existingCategory = await _context.Categories.FirstOrDefaultAsync(c => c.category_id == categoryDto.CategoryId);
            if (existingCategory == null)
            {
                return NotFound($"Category with ID {categoryDto.CategoryId} not found.");
            }

            existingCategory.status = "Inactive";
            existingCategory.modified_by = categoryDto.ModifiedBy;
            existingCategory.modified_date = DateTime.Now;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(existingCategory.category_id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }        
        }

    }
}
