using System.ComponentModel.DataAnnotations;

namespace VelesLibrary.DbModels
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        [Required]
        public byte[] PasswordHash { get; set; } = null!;
        [Required]
        public byte[] PasswordSalt { get; set; } = null!;
        [Required]
        public string Email { get; set; } = null!;

        public string? Avatar { get; set; }

        // Relation
        public ICollection<Group> Groups { get; set; } = null!;

    }
}
