using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VelesAPI.DbContext;
using VelesAPI.Interfaces;
using VelesLibrary.DbModels;
using VelesLibrary.DTOs;

namespace VelesAPI.Controllers;

public class GroupsController : BaseApiController
{
    private readonly IChatRepository _chatRepository;
    private readonly ChatDataContext _context;
    private readonly IGroupRepository _groupRepository;

    public GroupsController(ChatDataContext context, IChatRepository chatRepository, IGroupRepository groupRepository)
    {
        _context = context;
        _chatRepository = chatRepository;
        _groupRepository = groupRepository;
    }

    // GET: api/Groups
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Group>>> GetGroups()
    {
        if (_context.Groups == null)
        {
            return NotFound();
        }

        return await _context.Groups.ToListAsync();
    }

    // GET: api/Groups/Name/Karo
    [Authorize]
    [HttpGet("Name/{namePattern}")]
    public async Task<ActionResult<IEnumerable<Group>>> GetGroupsWithNameLike(string namePattern)
    {
        var groups = await _groupRepository.GetGroupsWithNameLikeAsync(namePattern);
        if (!groups.Any())
        {
            return NotFound();
        }

        return groups.ToList();
    }

    // GET: api/Groups/User/Karol
    [Authorize]
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
    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<Group>> GetGroup(int id)
    {

        var group = await _groupRepository.GetGroupWithIdAsync(id);

        if (group == null)
        {
            return NotFound();
        }

        return group;
    }

    // PUT: api/Groups/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /*[HttpPut("{id}")]
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
    }*/

    // POST: api/Groups
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<Group>> PostGroup(CreateGroupDto createGroupDto)
    {
        var existingGroup = await _groupRepository.GetGroupWithNameAsync(createGroupDto.Name);
        if (existingGroup != null)
        {
            return BadRequest(new ResponseDto { Status = ResponseStatus.Error, Message = "Group already exists" });
        }
        var group = new Group
        {
            Name = createGroupDto.Name, Connections = null, Users = null,
        };

        await _groupRepository.AddGroupAsync(group);

        if (await _groupRepository.SaveAllAsync())
        {
            return CreatedAtAction(nameof(GetGroup), new { id = group.Id }, group);
        }

        return Problem("Group was not created");
    }

    // DELETE: api/Groups/5
    /*[HttpDelete("{id}")]
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
    }*/

    private bool GroupExists(int id)
    {
        return (_context.Groups?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
