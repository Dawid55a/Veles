﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VelesAPI.Extensions;
using VelesAPI.Interfaces;
using VelesLibrary.DbModels;
using VelesLibrary.DTOs;

namespace VelesAPI.Controllers;

public class UsersController : BaseApiController
{
    private readonly IGroupRepository _groupRepository;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public UsersController(IUserRepository userRepository, IGroupRepository groupRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _groupRepository = groupRepository;
        _mapper = mapper;
    }
    /// <summary>
    /// Get all users
    /// </summary>
    /// <returns>List of userDto</returns>
    // GET: api/Users
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
    {
        var users = await _userRepository.GetUsersAsync();
        if (users == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<IEnumerable<UserDto>>(users));
    }
    /// <summary>
    /// Get user with id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>UserDto</returns>
    // GET: api/Users/5
    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUser(int id)
    {
        var user = await _userRepository.GetUserByIdAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<UserDto>(user));
    }
    /// <summary>
    /// Get users from group with name
    /// </summary>
    /// <param name="groupName"></param>
    /// <returns>List of userNames</returns>
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
    /// <summary>
    /// Change user nick in group
    /// </summary>
    /// <param name="changeNickInGroupDto"></param>
    /// <returns>ActionResult</returns>
    [Authorize]
    [HttpPut("change_nick")]
    public async Task<ActionResult> ChangeUserNameInGroup(ChangeNickInGroupDto changeNickInGroupDto)
    {
        var userId = User.GetUserId();
        switch (changeNickInGroupDto.Nick.Length)
        {
            case > 20:
                return BadRequest(new ResponseDto {Status = ResponseStatus.Error, Message = "Nick too long"});
            case < 3:
                return BadRequest(new ResponseDto {Status = ResponseStatus.Error, Message = "Nick too short"});
        }

        await _userRepository.ChangeNickInUserGroup(userId, changeNickInGroupDto.GroupId, changeNickInGroupDto.Nick);
        if (!await _userRepository.SaveAllAsync())
        {
            return BadRequest("Did not saved");
        }

        return Ok();
    }
}
