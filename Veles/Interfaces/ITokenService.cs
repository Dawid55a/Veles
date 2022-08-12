using VelesLibrary.DbModels;

namespace VelesAPI.Interfaces;

public interface ITokenService
{
    /// <summary>
    ///     Create token for User
    /// </summary>
    /// <param name="user">User</param>
    /// <returns>Token string</returns>
    string CreateToken(User user);
}
