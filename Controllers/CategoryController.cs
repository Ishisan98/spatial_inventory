﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using spatial_inventory_server.Data;
using spatial_inventory_server.Models;
using spatial_inventory_server.Dto;


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
        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAllCategories(int userId)
        {
            var categories = await _context.Categories
                            .Where(c => c.userId == userId)
                            .Select(c => new CategoryDto
                            {
                                CategoryId = c.category_id,
                                CategoryName = c.category_name,
                                Description = c.description,
                                Status = c.status,
                                CreatedDate = c.created_date,
                                CreatedBy = c.created_by,
                                UserId = c.userId,
                                ModifiedDate = c.modified_date,
                                ModifiedBy = c.modified_by
                            })
                            .ToListAsync();

            return Ok(categories);
        }


        // get all active categories
        [HttpGet("active-categories/{userId}")]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAllActiveCategories(int userId)
        {
            var activeCategories = await _context.Categories
                            .Where(c => c.status == "Active" && c.userId == userId)
                            .Select(c => new CategoryDto
                            {
                                CategoryId = c.category_id,         
                                CategoryName = c.category_name,     
                                Description = c.description,      
                                Status = c.status,                 
                                CreatedDate = c.created_date,     
                                CreatedBy = c.created_by,
                                UserId = c.userId,
                                ModifiedDate = c.modified_date,
                                ModifiedBy = c.modified_by
                            })
                            .ToListAsync();

            return Ok(activeCategories);
        }


        // get a category by ID
        [HttpGet("categoryById")]
        public async Task<ActionResult<CategoryDto>> GetCategoryById(CategoryDto category)
        {
            var existingCategory = await _context.Categories.FirstOrDefaultAsync(c => c.category_id == category.CategoryId && c.userId == category.UserId);

            if (existingCategory == null)
            {
                return NotFound();
            }

            var categoryDto = new CategoryDto
            {
                CategoryId = existingCategory.category_id,
                CategoryName = existingCategory.category_name,
                Description = existingCategory.description,
                Status = existingCategory.status,
                UserId = existingCategory.userId,
                CreatedDate = existingCategory.created_date,
                CreatedBy = existingCategory.created_by,
                ModifiedDate = existingCategory.modified_date,
                ModifiedBy = existingCategory.modified_by
            };

            return Ok(categoryDto);
        }


        // create a new category
        [HttpPost]
        public async Task<ActionResult<CategoryDto>> CreateCategory(CategoryDto categoryDto)
        {
            if (categoryDto.UserId == null || categoryDto.UserId == 0)
            {
                return BadRequest("Empty User");
            }

            var newCategory = new Category
            {
                category_name = categoryDto.CategoryName,
                description = categoryDto.Description,
                userId = categoryDto.UserId,
                created_by = categoryDto.CreatedBy,
                created_date = DateTime.Now
            };
            try
            {
                _context.Categories.Add(newCategory);
                await _context.SaveChangesAsync();
                return Ok(newCategory.category_id);
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

            var existingCategory = await _context.Categories.FirstOrDefaultAsync(c => c.category_id == categoryDto.CategoryId && c.userId == categoryDto.UserId);
            if (existingCategory == null)
            {
                return NotFound($"Category not found.");
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
