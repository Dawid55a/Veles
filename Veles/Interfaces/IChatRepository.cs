using VelesLibrary.DbModels;

namespace VelesAPI.Interfaces;

public interface IChatRepository
{
    void AddMessage(Message message);
    void RemoveMessage(Message message);
    Task<Group?> GetGroupForUserIdAsync(int id);
    Task<IEnumerable<Group>?> GetGroupsForUserAsync(User user);
    Task<IEnumerable<Group>?> GetGroupsForUserNameAsync(string username);
    Task<IEnumerable<Message>?> GetMessageThreadAsync(Group g);
    Task<Group?> GetGroupForConnectionAsync(string connection);
    void RemoveConnection(Connection connection);
    Task<bool> SaveAllAsync();
}
