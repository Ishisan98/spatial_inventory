using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using spatial_inventory_server.Data;
using spatial_inventory_server.Models;
using spatial_inventory_server.Dto;
using Microsoft.EntityFrameworkCore;

namespace spatial_inventory_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserLimitsController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public UserLimitsController(ApiDbContext context)
        {
            _context = context;
        }


        // get user limit by userId
        [HttpGet("{userId}")]
        public async Task<ActionResult<UserLimitsDto>> GetUserLimitsByUserId(int userId)
        {
            var userLimits = await _context.UsersLimits.FirstOrDefaultAsync(l => l.userId == userId);

            if (userLimits == null)
            {
                return NotFound();
            }

            var useLimitsDto = new UserLimitsDto
            {
                LimitId = userLimits.limit_id,
                UserId = userLimits.userId,
                CategoryLimit = userLimits.category_limit,
                ProductLimit = userLimits.product_limit
            };

            return Ok(useLimitsDto);    
        }


        // create new user limits
        [HttpPost]
        public async Task<ActionResult<UserLimitsDto>> CreateUserLimits(UserLimitsDto userLimitsDto)
        {
            var newUserLimits = new UserLimits
            {
                userId = userLimitsDto.UserId
            };

            try
            {
                _context.UsersLimits.Add(newUserLimits);
                await _context.SaveChangesAsync();
                return Ok(newUserLimits.limit_id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // update user limits
        [HttpPut]
        public async Task<ActionResult<UserLimitsDto>> UpdateUserLimits (UserLimitsDto userLimitsDto)
        {
            if (userLimitsDto == null || userLimitsDto.LimitId == 0)
            {
                return BadRequest();
            }

            var existingUserLimits = await _context.UsersLimits.FirstOrDefaultAsync(l => l.limit_id == userLimitsDto.LimitId);
            if (existingUserLimits == null)
            {
                return NotFound($"User Limits not found");
            }

            existingUserLimits.category_limit = userLimitsDto.CategoryLimit;
            existingUserLimits.product_limit = userLimitsDto.ProductLimit;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(existingUserLimits.limit_id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
    }
}
