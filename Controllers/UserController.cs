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
    public class UserController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public UserController(ApiDbContext context)
        {
            _context = context;
        }

        // get all users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
        {
            var users = await _context.Users
                        .Select(u => new UserDto
                        {
                            UserId = u.user_id,
                            Username = u.username,
                            Email = u.email,
                            CountryCode = u.country_code,
                            ContactNo = u.contact_no,
                            DisplayName = u.display_name,
                            Surname = u.surname,
                            FirstName = u.first_name,
                            LastName = u.last_name,
                            DateOfBirth = u.date_of_birth,
                            Gender = u.gender,
                            ProfilePicture = u.profile_picture,
                            CreatedDate = u.created_date,
                            ModifiedDate = u.modified_date
                        })
                        .ToListAsync();

            return Ok(users);
        }


        // get users by id
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserById(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.user_id == id);

            if (user == null)
            {
                return NotFound();
            }

            var userDto = new UserDto
            {
                UserId = user.user_id,
                Username = user.username,
                Email = user.email,
                CountryCode = user.country_code,
                ContactNo = user.contact_no,
                DisplayName = user.display_name,
                Surname = user.surname,
                FirstName = user.first_name,
                LastName = user.last_name,
                DateOfBirth = user.date_of_birth,
                Gender = user.gender,
                ProfilePicture = user.profile_picture,
                CreatedDate = user.created_date,
                ModifiedDate = user.modified_date
            };

            return Ok(userDto);
        }


        // user login
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> UserLogin(UserDto loginUser)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.username == loginUser.Username && u.password == loginUser.Password);

            if (user == null)
            {
                return NotFound();
            }

            var userDto = new UserDto
            {
                UserId = user.user_id,
                Username = user.username,
                Email = user.email,
                CountryCode = user.country_code,
                ContactNo = user.contact_no,
                DisplayName = user.display_name,
                Surname = user.surname,
                FirstName = user.first_name,
                LastName = user.last_name,
                DateOfBirth = user.date_of_birth,
                Gender = user.gender,
                ProfilePicture = user.profile_picture,
                CreatedDate = user.created_date,
                ModifiedDate = user.modified_date
            };

            return Ok(userDto);
        }


        // register new user
        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser(UserDto userDto)
        {
            var newUser = new User
            {
                username = userDto.Username,
                password = userDto.Password,
                email = userDto.Email,
                country_code = userDto.CountryCode,
                contact_no = userDto.ContactNo,
                display_name = userDto.DisplayName,
                surname = userDto.Surname,
                first_name = userDto.FirstName,
                last_name = userDto.LastName,
                date_of_birth = userDto.DateOfBirth,
                gender = userDto.Gender,
                profile_picture = userDto.ProfilePicture,
                created_date = DateTime.Now
            };
            try
            {
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();
                return Ok(newUser.user_id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpPut]
        public async Task<ActionResult<UserDto>> UpdateUser(UserDto userDto)
        {
            if (userDto == null || userDto.UserId == 0)
            {
                return BadRequest("Invalid User Data");
            }

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.user_id == userDto.UserId);
            if (existingUser == null)
            {
                return NotFound($"User with ID {userDto.UserId} not found");
            }

            existingUser.email = userDto.Email;
            existingUser.surname = userDto.Surname;
            existingUser.first_name = userDto.FirstName;
            existingUser.last_name = userDto.LastName;
            existingUser.country_code = userDto.CountryCode;
            existingUser.contact_no = userDto.ContactNo;
            existingUser.display_name = userDto.DisplayName;
            existingUser.date_of_birth = userDto.DateOfBirth;
            existingUser.gender = userDto.Gender;
            existingUser.profile_picture = userDto.ProfilePicture;
            existingUser.modified_date = userDto.ModifiedDate;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(existingUser.user_id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // deactivate user
        [HttpPut("deactivate-user")]
        public async Task<ActionResult<UserDto>> DeactivateUser(UserDto userDto)
        {
            if (userDto == null || userDto.UserId == 0)
            {
                return BadRequest("Invalid User Data");
            }

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.user_id == userDto.UserId);
            if (existingUser == null)
            {
                return NotFound($"User with ID {userDto.UserId} not found");
            }

            existingUser.status = "Inactive";
            existingUser.modified_date = DateTime.Now;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(existingUser.user_id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
