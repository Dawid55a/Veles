namespace VelesAPI.DbModels
{
    public class User
    {
        public int Id { get; set; }

        public string UserName { get; set; } = null!;

        public string Password { get; set; } = null!;
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public string Email { get; set; } = null!;

        public string Avatar { get; set; }

        // Relation
        public ICollection<Group> Groups { get; set; }

    }
}
