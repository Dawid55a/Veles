using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VelesAPI.Extensions;
using VelesAPI.Interfaces;
using VelesLibrary.DbModels;
using VelesLibrary.DTOs;

namespace VelesAPI.Controllers;

public class GroupsController : BaseApiController
{
    private readonly IChatRepository _chatRepository;
    private readonly IGroupRepository _groupRepository;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public GroupsController(IChatRepository chatRepository, IGroupRepository groupRepository,
        IUserRepository userRepository, IMapper mapper)
    {
        _chatRepository = chatRepository;
        _groupRepository = groupRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    // GET: api/Groups
    /*[Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Group>>> GetGroups()
    {
        if (_context.Groups == null)
        {
            return NotFound();
        }

        return await _context.Groups.ToListAsync();
    }*/

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

        return Ok(_mapper.Map<IEnumerable<GroupDto>>(groups));
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

        return Ok(_mapper.Map<IEnumerable<GroupDto>>(groups));
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
            return BadRequest(new ResponseDto {Status = ResponseStatus.Error, Message = "Group already exists"});
        }

        var group = new Group
        {
            Name = createGroupDto.Name, Connections = new List<Connection>(), UserGroups = new List<UserGroup>()
        };

        await _groupRepository.AddGroupAsync(group);

        var user = await _userRepository.GetUserByIdAsync(User.GetUserId());

        await _userRepository.AddUserToGroup(user!, group, Roles.Owner);

        if (await _groupRepository.SaveAllAsync())
        {
            return CreatedAtAction(nameof(GetGroup), new {id = group.Id}, group);
        }

        return Problem("Owner was not properly added to group");
    }

    // DELETE: api/Groups/5
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGroup(int id)
    {
        //TODO: check if user is an owner of this group
        var group = await _groupRepository.GetGroupWithIdAsync(id);
        if (group == null)
        {
            return NotFound();
        }

        _groupRepository.RemoveGroup(group);

        if (await _groupRepository.SaveAllAsync())
        {
            return Ok();
        }

        return BadRequest("Group was not removed");
    }
}
