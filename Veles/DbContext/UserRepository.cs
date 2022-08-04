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

    public async Task AddUserToGroup(User user, Group group)
    {
        var ug = new UserGroup() {User = user, Group = group, UserGroupNick = user.UserName};
        var userNew = await _context.Users.Include(u => u.UserGroups).Where(u => u.Equals(user)).FirstAsync();
        var groupNew = await _context.Groups.Include(g => g.UserGroups).Where(g => g.Equals(group)).FirstAsync();
        userNew.UserGroups.Add(ug);
        groupNew.UserGroups.Add(ug);
    }

    public async Task<IEnumerable<User>> GetUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _context.Users.SingleOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await _context.Users.SingleOrDefaultAsync(x => x.UserName == username.ToLower());
    }

    public async Task<IEnumerable<User>?> GetUsersForGroupName(string groupName)
    {
        var groupWithUser = await _context.Groups
            .Include(g => g.UserGroups)
            .ThenInclude(ug => ug.User)
            .FirstOrDefaultAsync(g => g.Name == groupName);
        var users = groupWithUser?.UserGroups.Select(ug => ug.User);
        return users;
    }
}
