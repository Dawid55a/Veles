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

    public async void AddUserAsync(User user)
    {
        await _context.Users.AddAsync(user);
    }

    public async Task<bool> SaveAllAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<IEnumerable<User>> GetUsersAsync()
    {
        return await _context.Users.Include(u => u.Groups).ToListAsync();
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await _context.Users.Include(u => u.Groups).SingleOrDefaultAsync(x => x.UserName == username.ToLower());
    }
}
