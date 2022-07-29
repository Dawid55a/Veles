using VelesLibrary.DbModels;

namespace VelesLibrary.DTOs
{
    public class MessageDto
    {
        public int Id { get; set; }
        public string? Text { get; set; }
        public DateTime CreatedDate { get; set; }
        public string User { get; set; } = null!;
    }
}
