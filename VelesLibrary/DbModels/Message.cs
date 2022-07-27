namespace VelesLibrary.DbModels
{
    public class Message
    {
        public int Id { get; set; }
        public string? Text { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public User User { get; set; } = null!;
        public Group Group { get; set; } = null!;
    }
}
