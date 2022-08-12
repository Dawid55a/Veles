using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using VelesLibrary.DTOs;

namespace VelesLibrary.DbModels;

// Relation many to many between user and group
public class UserGroup
{
    [Key, Column(Order = 1)]
    public int UserId { get; set; }
    [JsonIgnore]
    public User User { get; set; } = null!;
    public string UserGroupNick { get; set; } = string.Empty;
    public string Role { get; set; } = Roles.Member;
    [Key, Column(Order = 2)]
    public int GroupId { get; set; }
    [JsonIgnore]
    public Group Group { get; set; } = null!;
}