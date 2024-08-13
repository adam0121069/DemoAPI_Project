using CharlieApi.DapperService;
using CharlieApi.Models.DapperModels;
using Microsoft.AspNetCore.Mvc;

namespace CharlieApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DPUsersController : ControllerBase
{
    private readonly DapperUserService _dapperUserService;

    public DPUsersController(DapperUserService dapperUserService)
    {
        _dapperUserService = dapperUserService;
    }

    // GET: api/DPUsers
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DapperUser>>> GetUsers()
    {
        var users = await _dapperUserService.GetUsersAsync();
        return Ok(users);
    }

    // GET: api/DPUsers/5
    [HttpGet("{id}")]
    public async Task<ActionResult<DapperUser>> GetUser(int id)
    {
        var user = await _dapperUserService.GetUserByIdAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    // PUT: api/DPUsers/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, DapperUser user)
    {
        if (id != user.SeqNo)
        {
            return BadRequest();
        }

        var result = await _dapperUserService.UpdateUserAsync(user);

        if (result == 0)
        {
            return NotFound();
        }

        return NoContent();
    }

    // POST: api/DPUsers
    [HttpPost]
    public async Task<ActionResult<DapperUser>> CreateUser(DapperUser user)
    {
        // 假设你有一个方法来创建用户
        var result = await _dapperUserService.CreateUserAsync(user); 

        if (result == 0)
        {
            return BadRequest("Failed to create user.");
        }

        return CreatedAtAction(nameof(GetUser), new { id = user.SeqNo }, user);
    }

    // DELETE: api/DPUsers/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var result = await _dapperUserService.DeleteUserAsync(id);

        if (result == 0)
        {
            return NotFound();
        }

        return NoContent();
    }
}
