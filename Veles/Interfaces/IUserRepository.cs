using VelesLibrary.DbModels;

namespace VelesAPI.Interfaces;

public interface IUserRepository
{
    void Update(User user);
    void AddUserAsync(User user);
    Task<bool> SaveAllAsync();
    Task<IEnumerable<User>> GetUsersAsync();
    Task<User?> GetUserByIdAsync(int id);
    Task<User?> GetUserByUsernameAsync(string username);
}
