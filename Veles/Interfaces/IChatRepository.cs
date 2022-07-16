using VelesAPI.DbModels;
using VelesAPI.DTOs;

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
        Task<User> GetUser(string name);
        Task<Group> GetGroup(string name);

        Task<IEnumerable<MessageDto>> GetMessageThreadTask(Group g);
    }
}
