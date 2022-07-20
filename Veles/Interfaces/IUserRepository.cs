using VelesAPI.DbModels;

namespace VelesAPI.Interfaces
{
    public interface IUserRepository
    {
        void Update(User user);
        void AddUser(User user);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByUsernameAsync(string username);

        Task<Group> GetGroupByNameTask(string groupName);
    }
}
