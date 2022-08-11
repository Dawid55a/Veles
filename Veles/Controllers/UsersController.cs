using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VelesAPI.DbContext;
using VelesAPI.Extensions;
using VelesAPI.Interfaces;
using VelesLibrary.DbModels;
using VelesLibrary.DTOs;

namespace VelesAPI.Controllers;

public class UsersController : BaseApiController
{
    private readonly ChatDataContext _context;
    private readonly IGroupRepository _groupRepository;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public UsersController(ChatDataContext context, IUserRepository userRepository, IGroupRepository groupRepository,
        IMapper mapper)
    {
        _context = context;
        _userRepository = userRepository;
        _groupRepository = groupRepository;
        _mapper = mapper;
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

        return Ok(_mapper.Map<IEnumerable<UserDto>>(users));
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

        return Ok(_mapper.Map<UserDto>(user));
    }

    // GET: api/Users/Group/Karols
    [Authorize]
    [HttpGet("Group/{groupName}")]
    public async Task<ActionResult<IEnumerable<string>>> GetUsersForGroupName(string groupName)
    {
        var users = await _userRepository.GetUsersForGroupName(groupName);
        var group = await _groupRepository.GetGroupWithNameAsync(groupName);
        if (users == null)
        {
            return NotFound();
        }

        var nicks = new List<string>();

        foreach (var user in users)
        {
            var nick = user.UserGroups.Where(ug => ug.GroupId == group.Id && ug.UserId == user.Id)
                .Select(ug => ug.UserGroupNick);
            nicks.Add(nick.First());
        }

        return Ok(nicks);
    }

    [Authorize]
    [HttpPut("change_nick")]
    public async Task<ActionResult> ChangeUserNameInGroup(ChangeNickInGroupDto changeNickInGroupDto)
    {
        var userId = User.GetUserId();
        await _userRepository.ChangeNickInUserGroup(userId, changeNickInGroupDto.GroupId, changeNickInGroupDto.Nick);
        if (!await _userRepository.SaveAllAsync())
        {
            return BadRequest("Did not saved");
        }

        return Ok();
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
