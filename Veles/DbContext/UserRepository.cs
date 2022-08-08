using Microsoft.EntityFrameworkCore;
using VelesAPI.Interfaces;
using VelesLibrary.DbModels;
using VelesLibrary.DTOs;

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

    public async Task AddUserToGroup(User user, Group group, string role)
    {
        if (!(role == Roles.Member || role == Roles.Owner))
        {
            throw new ArgumentException("This role doesn't exist, check Roles class for correct roles");
        }
        var ug = new UserGroup() {User = user, Group = group, UserGroupNick = user.UserName, Role = role};
        var userGroup = await _context.UserGroups.FirstOrDefaultAsync(ug1 => ug1.User.Equals(user) && ug1.Group.Equals(group));
        if (userGroup != null)
        {
            return;
        }
        var userNew = await _context.Users.Include(u => u.UserGroups).Where(u => u.Equals(user)).FirstAsync();
        //var groupNew = await _context.Groups.Include(g => g.UserGroups).Where(g => g.Equals(group)).FirstAsync();
        userNew.UserGroups.Add(ug);
        group.UserGroups.Add(ug);
         

    }

    public async Task ChangeNickInUserGroup(int userId, int groupId, string nick)
    {
        var userNew = await _context.Users.Include(u => u.UserGroups).Where(u => u.Id == userId).FirstOrDefaultAsync();
        var groupNew = await _context.Groups.Where(g => g.Id == groupId).FirstOrDefaultAsync();
        var nickOld = userNew?.UserGroups.FirstOrDefault(ug => ug.Group.Equals(groupNew));
        nickOld.UserGroupNick = nick;
        _context.UserGroups.Update(nickOld);
    }

    public async Task<bool?> UserIsRemoved(int userId)
    {
        var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == userId);
        return user?.Removed;
    }

    public async Task<string?> GetUserRoleInGroup(int userId, int groupId)
    {
        var result = await _context.UserGroups.FirstOrDefaultAsync(ug => ug.UserId == userId && ug.GroupId == groupId);
        if (result == null)
        {
            throw new NullReferenceException("User role is null or group/user does not exist");
        }
        return result.Role;
    }

    public async Task<IEnumerable<User>> GetUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _context.Users.Include(u=> u.UserGroups).SingleOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await _context.Users.SingleOrDefaultAsync(x => x.UserName == username);
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
