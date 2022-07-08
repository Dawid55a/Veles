namespace Veles
{
    // Relation many to many between user and group
    public class UserGroup
    {
        public int UserId { get; set; }

        public User User { get; set; }

        public int GroupId { get; set; }

        public Group Group { get; set; }
    }
}

