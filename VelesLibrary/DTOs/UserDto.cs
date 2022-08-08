using VelesLibrary.DbModels;

namespace VelesLibrary.DTOs;

public class UserDto
{

    public int Id { get; set; }
    public string UserName { get; set; } = null!;
    public List<GroupNick> Nicks { get; set; } = null!;
    public string Email { get; set; } = null!;

}