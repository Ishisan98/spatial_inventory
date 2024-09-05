using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using spatial_inventory_server.Data;
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
        public async Task<ActionResult<IEnumerable<Category>>> GetAllCategories()
        {
            return await _context.Categories.ToListAsync();
        }


        // get all active categories
        [HttpGet("active-categories")]
        public async Task<ActionResult<IEnumerable<Category>>> GetAllActiveCategories()
        {
            var activeCategories = await _context.Categories
                                     .Where(c => c.status == "Active")
                                     .ToListAsync();

            return Ok(activeCategories);
        }


        // get a category by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategoryById(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return category;
        }


        // create a new category
        [HttpPost]
        public async Task<ActionResult<Category>> CreateCategory(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCategoryById), new { id = category.category_id }, category);
        }


        // update category
        [HttpPut]
        public async Task<ActionResult<Category>> UpdateCategory(Category category)
        {
            if (category == null || category.category_id == 0)
            {
                return BadRequest("Invalid category data.");
            }

            var existingCategory = await _context.Categories.FindAsync(category.category_id);
            if (existingCategory == null)
            {
                return NotFound($"Category with ID {category.category_id} not found.");
            }

            existingCategory.category_name = category.category_name;
            existingCategory.description = category.description;
            existingCategory.modified_by = category.modified_by;
            existingCategory.modified_date = DateTime.Now;

            await _context.SaveChangesAsync();

            return Ok(existingCategory);
        }


        // deactivate category
        [HttpPut]
        public async Task<ActionResult<Category>> DeactivateCategory(Category category)
        {
            if (category == null || category.category_id == 0)
            {
                return BadRequest("Invalid category Id.");
            }

            var existingCategory = await _context.Categories.FindAsync(category.category_id);
            if (existingCategory == null)
            {
                return NotFound($"Category with ID {category.category_id} not found.");
            }

            existingCategory.status = "Inactive";
            existingCategory.modified_by = category.modified_by;
            existingCategory.modified_date = DateTime.Now;

            await _context.SaveChangesAsync();

            return Ok(existingCategory);
        }
    }
}
