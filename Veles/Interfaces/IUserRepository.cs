using VelesAPI.DbModels;

namespace VelesAPI.Interfaces
{
    public interface IUserRepository
    {
        void Update(User user);
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByUsernameAsync(string username);

        Task<Group> GetGroupByNameTask(string groupName);
    }
}
