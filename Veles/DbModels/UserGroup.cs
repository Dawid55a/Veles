namespace VelesAPI.DbModels
{
    // Relation many to many between user and group
    public class UserGroup
    {
        public int UserId { get; set; }

        public User User { get; set; } = null!;

        public int GroupId { get; set; }

        public Group Group { get; set; } = null!;
    }
}

