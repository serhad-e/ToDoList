namespace todofinal.Application.DTOs;

public class UpdateTodoDto
{
    public int Id { get; set; } // Hangi satırı güncelleyeceğimizi bilmek için şart
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }
    }