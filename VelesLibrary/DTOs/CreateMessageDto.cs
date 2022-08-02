namespace VelesLibrary.DTOs;

public class CreateMessageDto
{
    public string Sender { get; set; } = null!;
    public string GroupName { get; set; } = null!;
    public string Content { get; set; } = null!;
    public DateTime Created { get; set; } = DateTime.UtcNow;
}