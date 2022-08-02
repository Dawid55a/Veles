namespace VelesLibrary.DTOs;

public class NewMessageDto
{
    public int Id { get; set; }
    public string? Text { get; set; }
    public DateTime CreatedDate { get; set; }
    public string User { get; set; } = null!;
    public string Group { get; set; } = null!;
}