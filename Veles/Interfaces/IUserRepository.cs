using VelesLibrary.DbModels;
using VelesLibrary.DTOs;

namespace VelesAPI.Interfaces;

public interface IUserRepository
{
    /// <summary>
    ///     Update User context
    /// </summary>
    /// <param name="user">User object</param>
    void Update(User user);

    /// <summary>
    ///     Add User asynchronously to databaseContext
    /// </summary>
    /// <param name="user">User object</param>
    /// <returns>Task</returns>
    Task AddUserAsync(User user);

    /// <summary>
    ///     Save all context to database
    /// </summary>
    /// <returns>Task returning true if some data was saved</returns>
    Task<bool> SaveAllAsync();

    /// <summary>
    /// Adding User to Group with group username set as user username
    /// </summary>
    /// <param name="user"></param>
    /// <param name="group"></param>
    /// <param name="role"></param>
    Task AddUserToGroup(User user, Group group, string role);

    /// <summary>
    ///     Get all users asynchronously
    /// </summary>
    /// <returns>IEnumerable of User</returns>
    Task<IEnumerable<User>> GetUsersAsync();

    /// <summary>
    ///     Get User by his Id with all his groups asynchronously
    /// </summary>
    /// <param name="id">User id</param>
    /// <returns>User or null if not found</returns>
    Task<User?> GetUserByIdAsync(int id);

    /// <summary>
    ///     Get User by his UserName with all his groups asynchronously
    /// </summary>
    /// <param name="username">User UserName</param>
    /// <returns>User or null if not found</returns>
    Task<User?> GetUserByUsernameAsync(string username);

    /// <summary>
    /// Get UserGroups in group with specified group name
    /// </summary>
    /// <param name="groupName">Name of group</param>
    /// <returns>List of users</returns>
    Task<IEnumerable<User>?> GetUsersForGroupName(string groupName);

    Task ChangeNickInUserGroup(int userId, int groupId, string nick);

    Task<bool?> UserIsRemoved(int userId);

    Task<string?> GetUserRoleInGroup(int userId, int groupId);
}
