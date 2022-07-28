using VelesLibrary.DbModels;

namespace VelesAPI.Interfaces;

public interface IGroupRepository
{
    void AddGroupAsync(Group group);
    void RemoveGroup(Group group);
    void UpdateGroup(Group group);
    Task<Group?> GetGroupWithNameAsync(string groupName);
    Task<Group?> GetGroupWithIdAsync(int groupId);

    public Task<bool> SaveAllAsync();
}
