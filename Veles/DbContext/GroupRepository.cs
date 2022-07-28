using Microsoft.EntityFrameworkCore;
using VelesAPI.Interfaces;
using VelesLibrary.DbModels;

namespace VelesAPI.DbContext;

public class GroupRepository : IGroupRepository
{
    private readonly ChatDataContext _context;

    public GroupRepository(ChatDataContext context)
    {
        _context = context;
    }

    public async void AddGroupAsync(Group group)
    {
        await _context.Groups.AddAsync(group);
    }

    public void RemoveGroup(Group group)
    {
        _context.Groups.Remove(group);
    }

    public void UpdateGroup(Group group)
    {
        _context.Entry(group).State = EntityState.Modified;
    }

    public async Task<Group?> GetGroupWithNameAsync(string groupName)
    {
        return await _context.Groups.FirstAsync(g => g.Name == groupName);
    }

    public async Task<Group?> GetGroupWithIdAsync(int groupId)
    {
        return await _context.Groups.FindAsync(groupId);
    }

    public async Task<bool> SaveAllAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
