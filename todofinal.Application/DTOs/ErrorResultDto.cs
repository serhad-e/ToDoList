namespace todofinal.Application.DTOs;

public class ErrorResultDto
{
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<string>? Errors { get; set; } // Validation hataları buraya gelecek
}