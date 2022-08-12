using VelesLibrary.DbModels;

namespace VelesAPI.Interfaces;

public interface IGroupRepository
{
    Task AddGroupAsync(Group group);
    void RemoveGroup(Group group);
    void UpdateGroup(Group group);
    Task AddConnectionAsync(Connection connection);
    Task<Group?> GetGroupWithNameAsync(string groupName);
    Task<IEnumerable<Group>?> GetGroupsWithNameLikeAsync(string namePattern);
    Task<Group?> GetGroupWithIdAsync(int groupId);
    Task<IEnumerable<Connection>> GetConnectionsAsync(string connectionString);
    void RemoveConnections(IEnumerable<Connection> connections);

    public Task<bool> SaveAllAsync();
}
