using Microsoft.AspNetCore.Mvc;
using CharlieApi.Models.EFModels;
using CharlieApi.EFService;

namespace CharlieApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EFUsersController : ControllerBase
{
    private readonly EFUserService _userService;

    public EFUsersController(EFUserService userService)
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
    public async Task<ActionResult<User>> PostUser([FromBody] User user)
    {
        if (user == null)
        {
            return BadRequest();
        }

        await _userService.CreateUserAsync(user);
        return CreatedAtAction(nameof(GetUser), new { id = user.SeqNo }, user);
    }

    // PUT: api/Users/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutUser(int id, [FromBody] User user)
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
