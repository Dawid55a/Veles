using System.Xml.Serialization;
using VelesLibrary.DTOs;
using VelesLibrary.DbModels;

namespace VelesAPI.Interfaces
{
    public interface IChatRepository
    {
        void AddGroup(Group group);
        void AddUser(User user);
        void AddMessage(Message message);
        void RemoveMessage(Message message);
        Task<User> GetUser(int id);
        Task<Group> GetGroup(int id);
        Task<Group> GetGroupForUserId(int id);
        Task<IEnumerable<Group>> GetGroupsForUserTask(User user);
        Task<IEnumerable<Group>> GetGroupsForUserNameTask(string username);
        Task<User> GetUser(string name);
        Task<Group> GetGroup(string name);
        Task<IEnumerable<Message>> GetMessageThreadForUserAndGroup(User user, Group group);
        Task<IEnumerable<IEnumerable<Message>>> MessageThreadsFromUsersGroups(User user);
        Task<IEnumerable<Message>> GetMessageThreadTask(Group g);
        Task<Group> GetGroupForConnection(string connection);
        void RemoveConnection(Connection connection);
        Task<bool> SaveAllAsync();

    }
}
