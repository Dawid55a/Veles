using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VelesAPI.DbContext;
using VelesAPI.Interfaces;
using VelesLibrary.DbModels;

namespace VelesAPI.Controllers;

public class UsersController : BaseApiController
{
    private readonly ChatDataContext _context;
    private readonly IUserRepository _userRepository;

    public UsersController(ChatDataContext context, IUserRepository userRepository)
    {
        _context = context;
        _userRepository = userRepository;
    }

    // GET: api/Users
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        var users = await _userRepository.GetUsersAsync();
        if (users == null)
        {
            return NotFound();
        }

        return Ok(users.ToList());
    }

    // GET: api/Users/5
    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {

        var user = await _userRepository.GetUserByIdAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }
    // GET: api/Users/Group/Karols
    [Authorize]
    [HttpGet("Group/{groupName}")]
    public async Task<ActionResult<IEnumerable<User>>> GetUsersForGroupName(string groupName)
    {
        var users = await _userRepository.GetUsersForGroupName(groupName);
        if (users == null)
        {
            return NotFound();
        }

        return Ok(users.ToList());
    }

    // NOT IMPLEMENTED
    // PUT: api/Users/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /*[HttpPut("{id}")]
    public async Task<IActionResult> PutUser(int id, User user)
    {
        if (id != user.Id)
        {
            return BadRequest();
        }

        _context.Entry(user).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!UserExists(id))
            {
                return NotFound();
            }

            throw;
        }

        return NoContent();
    }*/

    // DELETE: api/Users/5
    /*[HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        if (_context.UserGroups == null)
        {
            return NotFound();
        }

        var user = await _context.UserGroups.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        _context.UserGroups.Remove(user);
        await _context.SaveChangesAsync();

        return NoContent();
    }*/

    private bool UserExists(int id)
    {
        return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
