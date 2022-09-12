using Microsoft.EntityFrameworkCore;
using VelesAPI.Interfaces;
using VelesLibrary.DbModels;

namespace VelesAPI.DbContext;

public class ChatRepository : IChatRepository
{
    private readonly ChatDataContext _context;

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

    public async Task<IEnumerable<Group>?> GetGroupsForUserIdAsync(int userId)
    {
        var userWithGroups = await _context.Users
            .Where(u => u.Id == userId)
            .Include(u => u.UserGroups)
            .ThenInclude(ug => ug.Group)
            .FirstOrDefaultAsync();
        return userWithGroups?.UserGroups.Select(ug => ug.Group);
    }

    public async Task<IEnumerable<Group>?> GetGroupsForUserNameAsync(string username)
    {
        var userWithGroups = await _context.Users
            .Where(u => u.UserName == username)
            .Include(u => u.UserGroups)
            .ThenInclude(ug => ug.Group)
            .AsSplitQuery()
            .FirstOrDefaultAsync();
        return userWithGroups?.UserGroups.Select(ug => ug.Group);
    }

    public async Task<IEnumerable<Group>?> GetGroupsForUserIdIncludingConnectionsAsync(int id)
    {
        var userWithGroups = await _context.Users
            .Include(u => u.UserGroups)
            .ThenInclude(ug => ug.Group)
            .ThenInclude(g => g.Connections)
            .Where(u => u.Id == id)
            .AsSplitQuery()
            .FirstOrDefaultAsync();
        return userWithGroups?.UserGroups.Select(ug => ug.Group);
    }

    public async Task<IEnumerable<Message>?> GetMessageThreadAsync(Group g)
    {
        return await _context.Messages
            .Include(m => m.User)
            .Where(m => m.Group.Id == g.Id)
            .OrderBy(m => m.CreatedDate)
            .AsSplitQuery()
            .ToListAsync();
    }

    public async Task<bool> SaveAllAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
