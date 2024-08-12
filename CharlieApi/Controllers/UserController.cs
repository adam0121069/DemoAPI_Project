using Microsoft.AspNetCore.Mvc;
using CharlieApi.Models.EFModels;
using CharlieApi.EFService;

namespace CharlieApi.Controllers;
[ApiController]
[Route("[controller]")]

public class UsersController : ControllerBase
{
    private readonly UserService _userService;

    public UsersController(UserService userService)
    {
        _userService = userService;
    }

    // GET: api/Users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

    // GET: api/Users/5
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    // POST: api/Users
    [HttpPost]
    public async Task<ActionResult<User>> PostUser(User user)
    {
        var createdUser = await _userService.AddUserAsync(user);
        return CreatedAtAction(nameof(GetUser), new { id = createdUser.SeqNo }, createdUser);
    }

    // PUT: api/Users/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutUser(int id, User user)
    {
        if (id != user.SeqNo)
        {
            return BadRequest();
        }

        if (!await _userService.UserExistsAsync(id))
        {
            return NotFound();
        }

        await _userService.UpdateUserAsync(user);
        return NoContent();
    }

    // DELETE: api/Users/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        if (!await _userService.UserExistsAsync(id))
        {
            return NotFound();
        }

        await _userService.DeleteUserAsync(id);
        return NoContent();
    }
}

