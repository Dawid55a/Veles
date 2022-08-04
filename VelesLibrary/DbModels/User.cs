using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace VelesLibrary.DbModels;

public class User
{
    public int Id { get; set; }

    [Required] public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    [Required] public byte[] PasswordHash { get; set; } = null!;

    [Required] public byte[] PasswordSalt { get; set; } = null!;

    [EmailAddress] [Required] public string Email { get; set; } = null!;

    public string? Avatar { get; set; }

    // Relation
    [JsonIgnore]
    public ICollection<UserGroup> UserGroups { get; set; } = null!;
}