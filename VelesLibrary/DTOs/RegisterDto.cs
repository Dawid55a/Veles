using System.ComponentModel.DataAnnotations;

namespace VelesLibrary.DTOs;

public class RegisterDto
{
    [Required] public string UserName { get; set; } = null!;

    [Required] public string Password { get; set; } = null!;

    [Required] public string Email { get; set; } = null!;

    public string? Avatar { get; set; }
}