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
    /// <summary>
    /// Get all groups
    /// </summary>
    /// <returns>List of group</returns>
    // GET: api/Groups
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GroupDto>>> GetGroups()
    {
        var groups = await _groupRepository.GetGroups();
        if (groups == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<IEnumerable<GroupDto>>(groups));
    }
    /// <summary>
    /// Get groups based on patter working as Like in database
    /// </summary>
    /// <param name="namePattern"></param>
    /// <returns>List of groups</returns>
    // GET: api/Groups/Name/Karo
    [Authorize]
    [HttpGet("Name/{namePattern}")]
    public async Task<ActionResult<IEnumerable<GroupDto>>> GetGroupsWithNameLike(string namePattern)
    {
        var groups = await _groupRepository.GetGroupsWithNameLikeAsync(namePattern);
        if (!groups.Any())
        {
            return NotFound();
        }

        return Ok(_mapper.Map<IEnumerable<GroupDto>>(groups));
    }
    /// <summary>
    /// Get groups for user using username
    /// </summary>
    /// <param name="username"></param>
    /// <returns>List of groupDto</returns>
    // GET: api/Groups/User/Karol
    [Authorize]
    [HttpGet("User/{username}")]
    public async Task<ActionResult<IEnumerable<GroupDto>>> GetGroupsForUser(string username)
    {
        var groups = await _chatRepository.GetGroupsForUserNameAsync(username);
        if (groups == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<IEnumerable<GroupDto>>(groups));
    }
    /// <summary>
    /// Get group with id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Group</returns>
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
    /// <summary>
    /// Creating group as specified in createGroupDto and with owner as in passed token
    /// </summary>
    /// <param name="createGroupDto"></param>
    /// <returns>ActionResult</returns>
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<Group>> PostGroup(CreateGroupDto createGroupDto)
    {
        var existingGroup = await _groupRepository.GetGroupWithNameAsync(createGroupDto.Name);
        if (existingGroup != null)
        {
            return BadRequest(new ResponseDto {Status = ResponseStatus.Error, Message = "Group already exists"});
        }
        switch (createGroupDto.Name.Length)
        {
            case > 20:
                return BadRequest(new ResponseDto { Status = ResponseStatus.Error, Message = "Group name too long" });
            case < 3:
                return BadRequest(new ResponseDto { Status = ResponseStatus.Error, Message = "Group name too short" });
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
    /// <summary>
    /// Deleting group with specified id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>ActionResult</returns>
    // DELETE: api/Groups/5
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGroup(int id)
    {
        var role = await _userRepository.GetUserRoleInGroup(User.GetUserId(), id);
        if (role != Roles.Owner)
        {
            return BadRequest(new ResponseDto { Status = ResponseStatus.Error, Message = "You are not an owner of this group" });
        }

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

        return BadRequest(new ResponseDto { Status = ResponseStatus.Error, Message = "Group was removed" });
    }
}
