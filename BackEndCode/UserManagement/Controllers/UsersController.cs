using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserManagement.Models;
using UserManagement.Services;

namespace UserManagement.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpGet("GetUsers")]
        public async Task<ActionResult<IEnumerable<Users>>> GetUsers()
        {
            var users = await _usersService.GetAllUsers();
            return Ok(users);
        }

        [HttpGet("GetUserById")]
        public async Task<ActionResult<Users>> GetUserById(int id)
        {
            var user = await _usersService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost("CreateUser")]
        public async Task<ActionResult<Users>> CreateUser(Users user)
        {
            try
            {
                var newUser = await _usersService.AddUser(user);
                return CreatedAtAction(nameof(GetUserById), new { id = newUser.Id }, newUser);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser(int id, Users user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            try
            {
                await _usersService.UpdateUser(id, user);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }
        }

        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _usersService.DeleteUser(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpGet("GetActiveUsers")]
        public async Task<ActionResult<IEnumerable<Users>>> GetActiveUsers()
        {
            var activeUser = await _usersService.GetActiveUsers();
            return Ok(activeUser);
        }

        [HttpGet("SearchUsersByEmail")]
        public async Task<ActionResult<IEnumerable<Users>>> SearchUsersByEmail([FromQuery] string email)
        {
            var users = await _usersService.SearchUsersByEmail(email);
            return Ok(users);
        }

        [HttpPost("ActiveDeactive")]
        public async Task<IActionResult> ActiveDeactive(int id)
        {
            try
            {
                await _usersService.IsActive(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }


    }
}
