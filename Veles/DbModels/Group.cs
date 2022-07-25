namespace VelesAPI.DbModels
{
    public class Group
    {
        public int Id { get; set; }

        public string Name { get; set; }

        // Relation
        public ICollection<User> Users { get; set; }


    }
}
