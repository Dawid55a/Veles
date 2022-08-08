using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

    // GET: api/Messages
    /*[Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Message>>> GetMessages()
    {
        if (_context.Messages == null)
        {
            return NotFound();
        }

        return await _context.Messages.ToListAsync();
    }*/

    // GET: api/Messages/Group/Karols
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


    // GET: api/Messages/5
    /*[Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<Message>> GetMessage(int id)
    {
        if (_context.Messages == null)
        {
            return NotFound();
        }

        var message = await _context.Messages.FindAsync(id);

        if (message == null)
        {
            return NotFound();
        }

        return message;
    }*/

    // PUT: api/Messages/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /*[HttpPut("{id}")]
    public async Task<IActionResult> PutMessage(int id, Message message)
    {
        if (id != message.Id)
        {
            return BadRequest();
        }

        _context.Entry(message).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!MessageExists(id))
            {
                return NotFound();
            }

            throw;
        }

        return NoContent();
    }*/

    // POST: api/Messages
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /*[HttpPost]
    public async Task<ActionResult<Message>> PostMessage(Message message)
    {
        if (_context.Messages == null)
        {
            return Problem("Entity set 'ChatDataContext.Messages'  is null.");
        }

        _context.Messages.Add(message);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetMessage), new {id = message.Id}, message);
    }*/

    // DELETE: api/Messages/5
    /*[HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMessage(int id)
    {
        if (_context.Messages == null)
        {
            return NotFound();
        }

        var message = await _context.Messages.FindAsync(id);
        if (message == null)
        {
            return NotFound();
        }

        _context.Messages.Remove(message);
        await _context.SaveChangesAsync();

        return NoContent();
    }*/

    private bool MessageExists(int id)
    {
        return (_context.Messages?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
