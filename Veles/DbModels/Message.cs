namespace VelesAPI.DbModels
{
    public class Message
    {
        public int Id { get; set; }
        public string? Text { get; set; }
        public DateTime CreatedDate { get; set; }
        public User User { get; set; }
        public Group Group { get; set; }
    }
}
