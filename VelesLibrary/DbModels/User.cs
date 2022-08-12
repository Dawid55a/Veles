using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace VelesLibrary.DbModels;

public class User
{
    public int Id { get; set; }

    [Required] [MaxLength(20)] [MinLength(3)] public string UserName { get; set; } = null!;

    [Required] public byte[] PasswordHash { get; set; } = null!;

    [Required] public byte[] PasswordSalt { get; set; } = null!;

    [Required]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string Email { get; set; } = null!;

    public bool Removed { get; set; } = false;
    // Relation
    [JsonIgnore]
    public ICollection<UserGroup> UserGroups { get; set; } = null!;
}