namespace VelesLibrary.DTOs;

public class ResponseDto
{
    public ResponseStatus Status { get; set; } = ResponseStatus.Success;
    public string Message { get; set; } = string.Empty;
}