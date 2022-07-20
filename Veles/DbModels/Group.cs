namespace VelesAPI.DbModels
{
    public class Group
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        // Relation
        public ICollection<User> Users { get; set; } = null!;

        public ICollection<Connection> Connections { get; set; } = null!;
    }
}
