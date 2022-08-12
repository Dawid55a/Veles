using System.ComponentModel.DataAnnotations;

namespace VelesLibrary.DTOs;

public class CreateGroupDto
{
    [Required] public string Name { get; set; } = null!;
}