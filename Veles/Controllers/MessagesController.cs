using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VelesAPI.DbContext;
using VelesAPI.Interfaces;
using VelesLibrary.DbModels;
using VelesLibrary.DTOs;

namespace VelesAPI.Controllers;

public class MessagesController : BaseApiController
{
    private readonly IChatRepository _chatRepository;
    private readonly ChatDataContext _context;
    private readonly IGroupRepository _groupRepository;
    private readonly IMapper _mapper;

    public MessagesController(ChatDataContext context, IChatRepository chatRepository, IGroupRepository groupRepository,
        IMapper mapper)
    {
        _context = context;
        _chatRepository = chatRepository;
        _groupRepository = groupRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Gets messages for group name
    /// </summary>
    /// <param name="groupName"></param>
    /// <returns>List of messages</returns>
    [Authorize]
    [HttpGet("Group/{groupName}")]
    public async Task<ActionResult<IEnumerable<Message>>> GetMessagesForGroup(string groupName)
    {
        var group = await _groupRepository.GetGroupWithNameAsync(groupName);
        if (group == null)
        {
            return BadRequest("Group does not exist");
        }

        var messages = await _chatRepository.GetMessageThreadAsync(group);
        if (messages == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<IEnumerable<NewMessageDto>>(messages));
    }

}
