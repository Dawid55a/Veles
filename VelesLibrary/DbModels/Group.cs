using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace VelesLibrary.DbModels;

public class Group
{
    public int Id { get; set; }
    [MaxLength(20)] [MinLength(3)] public string Name { get; set; } = null!;
    // Relation
    [JsonIgnore] public virtual ICollection<UserGroup> UserGroups { get; set; } = null!;
    public ICollection<Connection> Connections { get; set; } = null!;
}