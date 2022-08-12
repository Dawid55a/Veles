using VelesLibrary.DbModels;

namespace VelesAPI.Interfaces;

public interface IChatRepository
{
    Task AddMessage(Message message);
    void RemoveMessage(Message message);
    Task<IEnumerable<Group>?> GetGroupsForUserIdAsync(int userId);
    Task<IEnumerable<Group>?> GetGroupsForUserNameAsync(string username);
    Task<IEnumerable<Group>?> GetGroupsForUserIdIncludingConnectionsAsync(int id);
    Task<IEnumerable<Message>?> GetMessageThreadAsync(Group g);
    Task<bool> SaveAllAsync();
}
