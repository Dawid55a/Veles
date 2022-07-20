using VelesAPI.DbModels;

namespace VelesAPI.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}
