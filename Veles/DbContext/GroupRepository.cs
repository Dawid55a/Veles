﻿using Microsoft.EntityFrameworkCore;
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


    public async Task<IEnumerable<Group>> GetGroups()
    {
        return await _context.Groups.ToListAsync();
    }

    public async Task AddGroupAsync(Group group)
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

    public async Task AddConnectionAsync(Connection connection)
    {
        await _context.Connections.AddAsync(connection);
    }

    public async Task<Group?> GetGroupWithNameAsync(string groupName)
    {
        return await _context.Groups
            .Include(g => g.UserGroups)
            .Include(g => g.Connections)
            .AsSplitQuery()
            .FirstOrDefaultAsync(g => g.Name == groupName);
    }

    public async Task<IEnumerable<Group>?> GetGroupsWithNameLikeAsync(string namePattern)
    {
        var groups = from g in _context.Groups where g.Name.Contains(namePattern) select g;
        return await groups.ToListAsync();
    }

    public async Task<Group?> GetGroupWithIdAsync(int groupId)
    {
        return await _context.Groups.Include(g => g.Connections).FirstOrDefaultAsync(g => g.Id == groupId);
    }

    public async Task<IEnumerable<Connection>> GetConnectionsAsync(string connectionString)
    {
        return await _context.Connections
            .Include(c => c.Group)
            .Where(c => c.ConnectionString == connectionString)
            .ToListAsync();
    }

    public void RemoveConnections(IEnumerable<Connection> connections)
    {
        _context.Connections.RemoveRange(connections);
    }

    public async Task<bool> SaveAllAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
