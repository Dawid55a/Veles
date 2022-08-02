using Microsoft.EntityFrameworkCore;
using VelesAPI.Interfaces;
using VelesLibrary.DbModels;

namespace VelesAPI.DbContext;

public class UserRepository : IUserRepository
{
    private readonly ChatDataContext _context;

    public UserRepository(ChatDataContext context)
    {
        _context = context;
    }

    public void Update(User user)
    {
        _context.Entry(user).State = EntityState.Modified;
    }

    public async Task AddUserAsync(User user)
    {
        await _context.Users.AddAsync(user);
    }

    public async Task<bool> SaveAllAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<IEnumerable<User>> GetUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _context.Users.FirstAsync(u => u.Id == id);
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await _context.Users.SingleOrDefaultAsync(x => x.UserName == username.ToLower());
    }

    public async Task<IEnumerable<User>> GetUsersForGroupName(string groupName)
    {
        return await _context.Users
            .Where(u => u.Groups
                .Any(g => g.Name == groupName))
            .ToListAsync();
    }
}
