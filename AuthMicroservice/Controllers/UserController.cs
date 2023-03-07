using AuthMicroservice.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using AutoMapper;

namespace AuthMicroservice.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly SignInManager<User> _signInManager;

        public UserController(IUserService userService, IMapper mapper, SignInManager<User> signInManager)
        {
            _userService = userService;
            _mapper = mapper;
            _signInManager = signInManager;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> RegisterAsync(CreateUserDto createUserDto)
        {
            // Create a new user based on the provided DTO
            var user = await _userService.CreateUserAsync(createUserDto);
            // Return a 201 Created response with the location of the newly created user
            return CreatedAtAction(nameof(GetUserAsync), new { id = user.Id }, user);
        }
        [HttpGet("login")] ///////Тестовый рут ууууудааааалллииитььь!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        public ActionResult TestGet() 
        {
            return Ok("fdgdfgdfg"); 
        }
        [HttpPost("login")]
        public async Task<ActionResult> LoginAsync(LoginDto loginDto)
        {
            // Attempt to sign in the user using their email and password
            var result = await _signInManager.PasswordSignInAsync(loginDto.Email, loginDto.Password, false, false);

            if (result.Succeeded)
            {
                // Return a 200 OK response if the sign in was successful
                return Ok();
            }

            // Return a 401 Unauthorized response if the sign in failed
            return Unauthorized();
        }

        [HttpGet("{id}")]
        [Authorize] // Require authentication for this endpoint
        public async Task<ActionResult<UserDto>> GetUserAsync(int id)
        {
            // Retrieve the user with the specified ID
            var user = await _userService.GetUserAsync(id);
            // Return a 200 OK response with the user's data
            return Ok(user);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")] // Require authentication and the "Admin" role for this endpoint
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsersAsync()
        {
            // Retrieve all users
            var users = await _userService.GetUsersAsync();
            // Return a 200 OK response with the users' data
            return Ok(users);
        }

        [HttpPut]
        [Authorize] // Require authentication for this endpoint
        public async Task<ActionResult> UpdateUserAsync(UpdateUserDto updateUserDto)
        {
            // Check if the user is authorized to update the specified user
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (userId != updateUserDto.Id)
            {
                // Return a 401 Unauthorized response if the user is not authorized
                return Unauthorized();
            }

            // Update the specified user
            await _userService.UpdateUserAsync(updateUserDto);
            // Retrieve the updated user's data
            var user = await _userService.GetUserAsync(userId);
            // Return a 200 OK response with the updated user's data
            return Ok(user);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // Require authentication and the "Admin" role for this endpoint
        public async Task<ActionResult> DeleteUserAsync(int id)
        {
            // Delete the user with the specified ID
            await _userService.DeleteUserAsync(id);
            // Return a 204 No Content response
            return NoContent();
        }
    }
}
