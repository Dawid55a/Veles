using AutoMapper;
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
        var userWithGroups = await _context.Users.Include(u => u.Groups).Where(u => u.Id == userId).ToListAsync();
        return userWithGroups[0].Groups;
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
            .ToListAsync();
    }

    public async Task<IEnumerable<Message>?> GetMessageThreadAsync(Group g)
    {
        return await _context.Messages
            .Include(m => m.User)
            .Where(m => m.Group.Id == g.Id)
            .ToListAsync();
    }

    public async Task<bool> SaveAllAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
