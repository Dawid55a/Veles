using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VelesAPI.DbContext;
using VelesAPI.Interfaces;
using VelesLibrary.DbModels;

namespace VelesAPI.Controllers;

public class GroupsController : BaseApiController
{
    private readonly ChatDataContext _context;
    private readonly IChatRepository _chatRepository;

    public GroupsController(ChatDataContext context, IChatRepository chatRepository)
    {
        _context = context;
        _chatRepository = chatRepository;
    }

    // GET: api/Groups
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Group>>> GetGroups()
    {
        if (_context.Groups == null)
        {
            return NotFound();
        }

        return await _context.Groups.ToListAsync();
    }

    // GET: api/Groups/User/Karol
    [HttpGet("User/{username}")]
    public async Task<ActionResult<IEnumerable<Group>>> GetGroupsForUser(string username)
    {
        var groups = await _chatRepository.GetGroupsForUserNameAsync(username);
        if (groups == null)
        {
            return NotFound();
        }
        return groups.ToList();
    }

    // GET: api/Groups/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Group>> GetGroup(int id)
    {
        if (_context.Groups == null)
        {
            return NotFound();
        }

        var group = await _context.Groups.FindAsync(id);

        if (group == null)
        {
            return NotFound();
        }

        return group;
    }

    // PUT: api/Groups/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutGroup(int id, Group group)
    {
        if (id != group.Id)
        {
            return BadRequest();
        }

        _context.Entry(group).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!GroupExists(id))
            {
                return NotFound();
            }

            throw;
        }

        return NoContent();
    }

    // POST: api/Groups
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Group>> PostGroup(Group group)
    {
        if (_context.Groups == null)
        {
            return Problem("Entity set 'ChatDataContext.Groups'  is null.");
        }

        _context.Groups.Add(group);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetGroup), new {id = group.Id}, group);
    }

    // DELETE: api/Groups/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGroup(int id)
    {
        if (_context.Groups == null)
        {
            return NotFound();
        }

        var group = await _context.Groups.FindAsync(id);
        if (group == null)
        {
            return NotFound();
        }

        _context.Groups.Remove(group);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool GroupExists(int id)
    {
        return (_context.Groups?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
