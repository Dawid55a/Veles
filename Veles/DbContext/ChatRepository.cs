using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VelesAPI.Interfaces;
using VelesLibrary.DbModels;

namespace VelesAPI.DbContext;

public class ChatRepository : IChatRepository
{
    private readonly ChatDataContext _context;
    private readonly IMapper _mapper;

    public ChatRepository(ChatDataContext context)
    {
        _context = context;
    }

    public async Task AddMessage(Message message)
    {
        await _context.Messages.AddAsync(message);
    }

    public void RemoveMessage(Message message)
    {
        _context.Messages.Remove(message);
    }

    public async Task<Group?> GetGroupForUserIdAsync(int id)
    {
        // TODO: Returns only first group
        var user = await _context.Users.FindAsync(id);
        var groups = await (from g in _context.Groups where g.Users.Any(u => u.Id == id) select g).ToListAsync();
        return groups[0];
    }

    public async Task<IEnumerable<Group>?> GetGroupsForUserAsync(User user)
    {
        return await _context.Groups.Where(g => g.Users.Any(u => u.Id == user.Id)).ToListAsync();
    }

    public async Task<IEnumerable<Group>?> GetGroupsForUserNameAsync(string username)
    {
        var user = await _context!.Users!.SingleOrDefaultAsync(x => x.UserName == username.ToLower());
        if (user == null)
        {
            return null;
        }

        return await _context.Groups
            .Where(g => g.Users
                .Any(u => u.Id == user.Id))
            .ToListAsync();
    }

    public async Task<IEnumerable<Group>?> GetGroupsForUserIdIncludingConnectionsAsync(int id)
    {
        var user = await _context!.Users!.SingleOrDefaultAsync(x => x.Id == id);
        if (user == null)
        {
            return null;
        }

        return await _context.Groups
            .Include(g => g.Connections)
            .Where(g => g.Users
                .Any(u => u.Id == user.Id))
            .ToListAsync(); ;
    }

    public async Task<IEnumerable<Message>?> GetMessageThreadAsync(Group g)
    {
        var messages = await _context.Messages
            .Where(m => m.Group.Id == g.Id)
            .ToListAsync();
        return messages;
    }

    public async Task<Group?> GetGroupForConnectionAsync(string connection)
    {

        var q = await _context.Connections
            .Include(c=>c.Group)
            .Where(c => c.ConnectionString == connection)
            .ToListAsync();
        /*if (query.Count > 1)
        {
            throw new ArgumentOutOfRangeException("Returned more than one group! Should be one");
        }*/

        return q[0].Group;
    }

    public void RemoveConnection(Connection connection)
    {
        _context.Connections.Remove(connection);
    }

    public async Task<bool> SaveAllAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
