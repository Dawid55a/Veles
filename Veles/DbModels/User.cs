namespace VelesAPI.DbModels
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Avatar { get; set; } = null!;

        // Relation
        public ICollection<Group> Groups { get; set; } = null!;

    }
}
